using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.AllUser)]
    public class OrderController : CustomerController
    {
        // GET: Order
        public ActionResult Index(UserInfo user, int storeId = 0, int brandId = 0, int orderStatus = 0,
            string beginDateStr = "",
            string endDateStr = "",
            string keyWords = "", int page = 1,
            int pageSize = 20)
        {
            DateTime beginDate, endDate;

            if (!DateTime.TryParse(beginDateStr, out beginDate))
            {
                beginDate = DateTime.MinValue;
            }
            if (!DateTime.TryParse(endDateStr, out endDate))
            {
                endDate = DateTime.Now;
            }
            endDate = endDate.AddDays(1).AddSeconds(-1);

            var userId = user.UserId;
            if (user.UserRole == AgentEnums.UserRole.Administrator || user.UserRole == AgentEnums.UserRole.Staff)
            {
                userId = 0;
            }

            int totalItem;
            var list = OrderService.Instance.SearchOrderList(userId, storeId, orderStatus, beginDate, endDate, keyWords,
                page,
                pageSize, out totalItem);

            ViewBag.ProductBase = ProductService.Instance.GetProductBaseList(
                list.SelectMany(t => t.OrderDetails).Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = ProductService.Instance.GetProduct(
                list.SelectMany(t => t.OrderDetails).Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            ViewBag.brandId = brandId;
            ViewBag.beginDateStr = beginDateStr;
            ViewBag.endDateStr = endDateStr;
            ViewBag.keyWords = keyWords;
            ViewBag.storeId = storeId;

            var data = new ApiPageList<OrderMaster>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            };

            return View(data);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Address(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Content("未找到指定的订单");
            }
            if (order.OrderStatus == AgentEnums.MasterOrderStatus.已完成 ||
                order.OrderStatus == AgentEnums.MasterOrderStatus.已发货)
            {
                return Content("当前订单已发货.不能修改地址");
            }
            if (user.UserRole == AgentEnums.UserRole.StoreUser && order.UserId != user.UserId)
            {
                return Content("未找到指定的订单");
            }

            return View(order);
        }

        /// <summary>
        /// 订单备注
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult Note(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Content("未找到指定的订单");
            }

            if (user.UserRole == AgentEnums.UserRole.StoreUser && order.UserId != user.UserId)
            {
                return Content("未找到指定的订单");
            }

            ViewBag.OrderLogs = OrderLogsService.Instance.GetList(orderId).ToList();

            return View(order);
        }

        public ActionResult Detail(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Content("未找到指定的订单");
            }

            if (user.UserRole == AgentEnums.UserRole.StoreUser && order.UserId != user.UserId)
            {
                return Content("未找到指定的订单");
            }

            ViewBag.ProductBase = ProductService.Instance.GetProductBaseList(
                order.OrderDetails.Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = ProductService.Instance.GetProduct(
                order.OrderDetails.Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            var orderLogistics = LogisticsService.Instance.GetLogistics(orderId);

            foreach (var item in order.OrderDetails)
            {
                foreach (var logistics in orderLogistics)
                {
                    if (logistics.FlowIdList.Contains(item.FlowId))
                    {
                        item.LogisticsInfo = logistics;
                    }
                }
            }

            ViewBag.OrderLogs = OrderLogsService.Instance.GetList(orderId);

            return View(order);
        }


        public ActionResult SendGood(UserInfo user, long orderId)
        {
            var orderInfo = OrderService.Instance.Get(new { orderId });

            if (orderInfo.OrderStatus == AgentEnums.MasterOrderStatus.待发货)
            {
                return Content("只有待发货状态的订单,才能发货");
            }

            ViewBag.ComplanyList = LogisticsService.Instance.GetComplanyList();

            return View(orderInfo);
        }

        #region Ajax

        [HttpPost]
        public JsonResult CreateOrder(UserInfo user, OrderMaster order)
        {
            if (string.IsNullOrWhiteSpace(order?.OrderNo))
            {
                return Json(new ApiResult<int>(2) { Ret = RetEum.ApplicationError, Message = "数据错误" });
            }
            if (OrderService.Instance.IsExists(order.OrderNo))
            {
                return Json(new ApiResult<int>(3) { Ret = RetEum.ApplicationError, Message = "订单号已经存在,不能重复添加" });
            }

            var storeInfo = StoreService.Instance.GetStoreInfo(new { storeId = order.StoreId, status = 0 });
            if (storeInfo == null)
            {
                return Json(new ApiResult<int>(4) { Ret = RetEum.ApplicationError, Message = "店铺信息错误" });
            }

            if (user.UserRole == AgentEnums.UserRole.StoreUser && storeInfo.MasterUserId != user.UserId)
            {
                return Json(new ApiResult<int>(4) { Ret = RetEum.ApplicationError, Message = "店铺信息错误" });
            }

            order.IsSelfSupport = storeInfo.IsSelfSupport;
            order.UserId = user.UserId;
            order.StoreName = storeInfo.StoreName;

            foreach (var item in order.OrderDetails)
            {
                var stock = ProductStockService.Instance.Get(new { item.ProductId, item.SkuId });
                if (stock == null || stock.StockNum < 1 || item.Quantity > stock.StockNum)
                {
                    return Json(new ApiResult<int>(5) { Ret = RetEum.ApplicationError, Message = "库存不足" });
                }
            }

            var result = OrderService.Instance.CreateOrder(user, order, order.OrderDetails.ToList());

            return Json(new ApiResult<bool>(result) { Message = result ? order.OrderId.ToString() : "操作失败" });
        }

        [HttpPost]
        public ActionResult CreateNote(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Content("未找到指定的订单");
            }
            if (user.UserRole == AgentEnums.UserRole.StoreUser && order.UserId != user.UserId)
            {
                return Content("未找到指定的订单");
            }

            var content = Request.Form["content"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 1, Message = "备注内容不能为空" });
            }

            var result = OrderLogsService.Instance.CreateLog(new OrderLogs()
            {
                OrderId = orderId,
                UserId = user.UserId,
                UserName = user.UserName,
                Title = "添加备注",
                Content = content,
                CreateDate = DateTime.Now
            });

            return Json(new
            {
                userName = user.UserName,
                title = "添加备注",
                createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        //取消订单
        [HttpPost]
        public ActionResult CancelOrder(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 1, Message = "未找到指定的订单" });
            }
            if (user.UserRole == AgentEnums.UserRole.StoreUser)
            {
                if (order.UserId != user.UserId)
                {
                    return Json(new ApiResult<int>(3) { ErrorCode = 1, Message = "未找到指定的订单" });
                }
                if (order.OrderStatus != AgentEnums.MasterOrderStatus.待支付)
                {
                    return Json(new ApiResult<int>(4) { ErrorCode = 1, Message = "只有待支付状态的订单才能取消" });
                }
            }
            if (order.OrderStatus == AgentEnums.MasterOrderStatus.已完成)
            {
                return Json(new ApiResult<int>(5) { ErrorCode = 1, Message = "已完成订单不能取消" });
            }

            var result = OrderService.Instance.UpdateOrder(new { orderStatus = AgentEnums.MasterOrderStatus.已取消 }, new { orderId });
            if (result)
            {
                OrderService.Instance.UpdateOrderDetail(new { orderStatus = AgentEnums.OrderDetailStatus.已取消 }, new { orderId });
                OrderLogsService.Instance.CreateLog(new OrderLogs
                {
                    OrderId = orderId,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Title = "取消订单",
                    Content = user.UserName + "取消了订单",
                    CreateDate = DateTime.Now
                });
            }
            return Json(new ApiResult<bool>(result));
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetDespatched(UserInfo user)
        {
            var orderId = Convert.ToInt32(Request.Form["orderId"] ?? string.Empty);

            var orderInfo = OrderService.Instance.Get(new { orderId });

            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 2, Message = "未找到订单" });
            }
            if (user.UserRole != AgentEnums.UserRole.Administrator && user.UserRole != AgentEnums.UserRole.Staff)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 3, Message = "没有操作权限" });
            }
            if (orderInfo.OrderStatus == AgentEnums.MasterOrderStatus.待发货)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 4, Message = "只有待发货状态的订单,才能发货" });
            }
            var logisticsId = Convert.ToInt32(Request.Form["logistics_id"] ?? string.Empty);
            var logisticsInfo = LogisticsService.Instance.GetComplanyList().First(t => t.Key == logisticsId);

            if (logisticsInfo.Key < 1)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 5, Message = "物流信息错误" });
            }
            var price = Convert.ToDecimal(Request.Form["postage"] ?? string.Empty);
            var oddNumber = Request.Form["shipping_code"] ?? string.Empty;
            var note = Request.Form["note"] ?? string.Empty;

            var result = OrderService.Instance.SendDeliverGoodsV2(user, orderInfo, new LogisticsInfo
            {
                OrderId = orderInfo.OrderId,
                CompanyId = logisticsInfo.Key,
                CompanyName = logisticsInfo.Value,
                Price = price,
                OddNumber = oddNumber,
                UpdateDate = DateTime.Now,
                CreateDate = DateTime.Now,
                Status = 0
            }, note);

            return Json(new ApiResult<bool>(result));
        }

        /// <summary>
        /// 缺货标记
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public ActionResult SetDefect(UserInfo user, long orderId, int flowId)
        {
            var orderInfo = OrderService.Instance.Get(new { orderId });
            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 2, Message = "未找到订单" });
            }
            if (orderInfo.OrderStatus == AgentEnums.MasterOrderStatus.已发货 || orderInfo.OrderStatus == AgentEnums.MasterOrderStatus.已完成)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 3, Message = "已发货和已完成的订单不能做无货标记" });
            }
            var flowInfo = orderInfo.OrderDetails.FirstOrDefault(t => t.FlowId == flowId);
            if (flowInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 4, Message = "参数flowId错误" });
            }
            if (flowInfo.OrderStatus == AgentEnums.OrderDetailStatus.已发货 || flowInfo.OrderStatus == AgentEnums.OrderDetailStatus.已取消)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 5, Message = "已发货和已取消的订单不能做无货标记" });
            }
            var result = OrderService.Instance.UpdateOrderDetail(new { orderStatus = AgentEnums.OrderDetailStatus.无货取消 }, new { flowId });
            if (result)
            {
                OrderLogsService.Instance.CreateLog(new OrderLogs
                {
                    OrderId = orderId,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Title = "缺货标记",
                    Content = user.UserName + "对商品" + flowInfo.ProductNo + "设置了缺货标记",
                    CreateDate = DateTime.Now
                });
            }
            return Json(new ApiResult<bool>(result));
        }
        #endregion
    }
}