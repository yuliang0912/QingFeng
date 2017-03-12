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
            var order = OrderService.Instance.Get(new {orderId});
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
            var order = OrderService.Instance.Get(new {orderId});
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
            var order = OrderService.Instance.Get(new {orderId});
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


        #region Ajax

        [HttpPost]
        public JsonResult CreateOrder(UserInfo user, OrderMaster order)
        {
            if (string.IsNullOrWhiteSpace(order?.OrderNo))
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据错误"});
            }
            if (OrderService.Instance.IsExists(order.OrderNo))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "订单号已经存在,不能重复添加"});
            }

            var storeInfo = StoreService.Instance.GetStoreInfo(new {storeId = order.StoreId, status = 0});
            if (storeInfo == null)
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "店铺信息错误"});
            }

            if (user.UserRole == AgentEnums.UserRole.StoreUser && storeInfo.MasterUserId != user.UserId)
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "店铺信息错误"});
            }

            order.IsSelfSupport = storeInfo.IsSelfSupport;
            order.UserId = user.UserId;
            order.StoreName = storeInfo.StoreName;

            foreach (var item in order.OrderDetails)
            {
                var stock = ProductStockService.Instance.Get(new {item.ProductId, item.SkuId});
                if (stock == null || stock.StockNum < 1 || item.Quantity > stock.StockNum)
                {
                    return Json(new ApiResult<int>(5) {Ret = RetEum.ApplicationError, Message = "库存不足"});
                }
            }

            var result = OrderService.Instance.CreateOrder(user, order, order.OrderDetails.ToList());

            return Json(new ApiResult<bool>(result) {Message = result ? order.OrderId.ToString() : "操作失败"});
        }

        [HttpPost]
        public ActionResult CreateNote(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new {orderId});
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
                return Json(new ApiResult<int>(2) {ErrorCode = 1, Message = "备注内容不能为空"});
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
        #endregion
    }
}