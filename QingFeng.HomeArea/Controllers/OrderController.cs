using ClosedXML.Excel;
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
    public class OrderController : CustomerController
    {
        // GET: Order
        [AdminAuthorize(AgentEnums.SubMenuEnum.订单列表)]
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
                user.StoreList = StoreService.Instance.GetList(new { status = 0 }).ToList();
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
            ViewBag.title = orderStatus == 0 ? "订单列表" : ((AgentEnums.MasterOrderStatus)orderStatus).ToString() + "订单";

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
        [AdminAuthorize(AgentEnums.SubMenuEnum.添加订单)]
        public ActionResult Add()
        {
            return View();
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.修改地址)]
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
        [AdminAuthorize(AgentEnums.SubMenuEnum.添加备注)]
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
        [AdminAuthorize(AgentEnums.SubMenuEnum.查看订单)]
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

        [AdminAuthorize(AgentEnums.SubMenuEnum.发货)]
        public ActionResult SendGood(UserInfo user, long orderId)
        {
            var orderInfo = OrderService.Instance.Get(new { orderId });

            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.待发货)
            {
                return Content("只有待发货状态的订单,才能发货");
            }

            ViewBag.ComplanyList = LogisticsService.Instance.GetComplanyList();

            return View(orderInfo);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.订单导出)]
        public ActionResult Export(UserInfo user)
        {
            var storeList = StoreService.Instance.GetList(new { status = 0 }).ToList();

            return View(storeList);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.订单导出)]
        public ActionResult ExportFile(UserInfo user, int storeId = 0, int brandId = 0, int orderStatus = 0,
            string beginDateStr = "", string endDateStr = "")
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

            int totalItem;
            var list =
                OrderService.Instance.SearchOrderList(0, storeId, orderStatus, beginDate, endDate, string.Empty, 1,
                    int.MaxValue, out totalItem).ToList();

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add("Sheet1");
            var workSheet = workbook.Worksheet(1);
            workSheet.Cell(1, 1).Value = "订单流水号";
            workSheet.Cell(1, 2).Value = "订单编号";
            workSheet.Cell(1, 3).Value = "店铺";
            workSheet.Cell(1, 4).Value = "联系人";
            workSheet.Cell(1, 5).Value = "联系电话";
            workSheet.Cell(1, 6).Value = "订单金额";
            workSheet.Cell(1, 7).Value = "订单状态";
            workSheet.Cell(1, 8).Value = "订单日期";
            workSheet.Cell(1, 10).Value = "品牌";
            workSheet.Cell(1, 10).Value = "货号";
            workSheet.Cell(1, 11).Value = "颜色";
            workSheet.Cell(1, 12).Value = "尺码";
            workSheet.Cell(1, 13).Value = "数量";
            workSheet.Cell(1, 14).Value = "单价";
            workSheet.Cell(1, 15).Value = "子订单状态";

            var rows = 2;
            foreach (var order in list)
            {
                workSheet.Cell(rows, 1).Value = order.OrderId;
                workSheet.Cell(rows, 2).Value = order.OrderNo;
                workSheet.Cell(rows, 3).Value = order.StoreName;
                workSheet.Cell(rows, 4).Value = order.ContactName;
                workSheet.Cell(rows, 5).Value = order.ContactPhone;
                workSheet.Cell(rows, 6).Value = order.OrderAmount;
                workSheet.Cell(rows, 7).Value = order.OrderStatus;
                workSheet.Cell(rows, 8).Value = order.CreateDate;

                workSheet.Range(rows, 1, rows - 1 + order.OrderDetails.Count(), 1).Merge();
                workSheet.Range(rows, 2, rows - 1 + order.OrderDetails.Count(), 2).Merge();
                workSheet.Range(rows, 3, rows - 1 + order.OrderDetails.Count(), 3).Merge();
                workSheet.Range(rows, 4, rows - 1 + order.OrderDetails.Count(), 4).Merge();
                workSheet.Range(rows, 5, rows - 1 + order.OrderDetails.Count(), 5).Merge();
                workSheet.Range(rows, 6, rows - 1 + order.OrderDetails.Count(), 6).Merge();
                workSheet.Range(rows, 7, rows - 1 + order.OrderDetails.Count(), 7).Merge();
                workSheet.Range(rows, 8, rows - 1 + order.OrderDetails.Count(), 8).Merge();

                workSheet.Cell(rows, 2).Value = order.OrderNo;
                workSheet.Cell(rows, 3).Value = order.StoreName;
                workSheet.Cell(rows, 4).Value = order.ContactName;
                workSheet.Cell(rows, 5).Value = order.ContactPhone;
                workSheet.Cell(rows, 6).Value = order.OrderAmount;
                workSheet.Cell(rows, 7).Value = order.OrderStatus;
                workSheet.Cell(rows, 8).Value = order.CreateDate;

                workSheet.Range(rows, 1, rows - 1 + order.OrderDetails.Count(), 1).Merge();
                workSheet.Range(rows, 2, rows - 1 + order.OrderDetails.Count(), 2).Merge();
                workSheet.Range(rows, 3, rows - 1 + order.OrderDetails.Count(), 3).Merge();
                workSheet.Range(rows, 4, rows - 1 + order.OrderDetails.Count(), 4).Merge();
                workSheet.Range(rows, 5, rows - 1 + order.OrderDetails.Count(), 5).Merge();
                workSheet.Range(rows, 6, rows - 1 + order.OrderDetails.Count(), 6).Merge();
                workSheet.Range(rows, 7, rows - 1 + order.OrderDetails.Count(), 7).Merge();
                workSheet.Range(rows, 8, rows - 1 + order.OrderDetails.Count(), 8).Merge();

                for (var j = 0; j < order.OrderDetails.Count(); j++)
                {
                    var orderDetail = order.OrderDetails.ToList()[j];

                    workSheet.Cell(j + rows, 9).Value = orderDetail.BrandId.ToString();
                    workSheet.Cell(j + rows, 10).Value = orderDetail.BaseNo;
                    workSheet.Cell(j + rows, 11).Value = orderDetail.ProductNo;
                    workSheet.Cell(j + rows, 12).Value = orderDetail.SkuName;
                    workSheet.Cell(j + rows, 13).Value = orderDetail.Quantity;
                    workSheet.Cell(j + rows, 14).Value = orderDetail.Price;
                    workSheet.Cell(j + rows, 15).Value = orderDetail.OrderStatus;
                }

                rows += order.OrderDetails.Count();
            }

            workSheet.Rows(1, 1000).Height = 20;
            workSheet.Columns(1, 100).Width = 25;
            workSheet.Range("A1:O1").Style.Fill.BackgroundColor = XLColor.Green;
            workSheet.Range("A1:O1").Style.Font.SetFontColor(XLColor.Yellow);
            workSheet.Range("A1:O1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workSheet.Range("A2:H100").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            workSheet.Range("A2:O100").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            return new Common.ActionResultExtensions.ExportExcelResult
            {
                workBook = workbook,
                FileName =
                    string.Concat("order-export-", beginDate.ToString("yyyy-MM-dd"), "-", endDate.ToString("yyyy-MM-dd"),
                        ".xlsx")
            };
        }

        #region Ajax

        [HttpPost]
        [AdminAuthorize(AgentEnums.SubMenuEnum.添加订单)]
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
        [AdminAuthorize(AgentEnums.SubMenuEnum.添加备注)]
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
        [HttpGet]
        [AdminAuthorize(AgentEnums.SubMenuEnum.取消订单)]
        public ActionResult CancelOrder(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new { orderId });
            if (order == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 2, Message = "未找到订单" });
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
        [AdminAuthorize(AgentEnums.SubMenuEnum.发货)]
        public ActionResult SetDespatched(UserInfo user)
        {
            var orderId = Convert.ToInt64(Request.Form["orderId"] ?? string.Empty);

            var orderInfo = OrderService.Instance.Get(new { orderId });

            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 2, Message = "未找到订单" });
            }
            if (user.UserRole != AgentEnums.UserRole.Administrator && user.UserRole != AgentEnums.UserRole.Staff)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 3, Message = "没有操作权限" });
            }
            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.待发货)
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
            if (price < 0)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 6, Message = "物流金额错误" });
            }
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
        /// <param name="orderId"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.SubMenuEnum.无货标记)]
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
            flowInfo.OrderStatus = AgentEnums.OrderDetailStatus.无货取消;

            //如果子订单全部取消,则主订单也设置为取消状态
            if (!orderInfo.OrderDetails.Any(t =>
                t.OrderStatus == AgentEnums.OrderDetailStatus.待发货 ||
                t.OrderStatus == AgentEnums.OrderDetailStatus.已发货 ||
                t.OrderStatus == AgentEnums.OrderDetailStatus.无货异常))
            {
                OrderService.Instance.UpdateOrder(new { orderStatus = AgentEnums.MasterOrderStatus.已取消.GetHashCode() },
                    new { orderId });
            }

            var result = OrderService.Instance.UpdateOrderDetail(new { orderStatus = AgentEnums.OrderDetailStatus.无货取消 }, new { flowId });
            if (result)
            {
                OrderLogsService.Instance.CreateLog(new OrderLogs
                {
                    OrderId = orderId,
                    UserId = user.UserId,
                    UserName = user.NickName,
                    Title = "缺货标记",
                    Content = user.UserName + "对商品" + flowInfo.ProductNo + "设置了缺货标记",
                    CreateDate = DateTime.Now
                });
            }
            return Json(new ApiResult<bool>(result));
        }


        /// <summary>
        /// 取消子订单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderId"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.SubMenuEnum.异常取消)]
        public ActionResult SetCancel(UserInfo user, long orderId, int flowId)
        {
            var orderInfo = OrderService.Instance.Get(new { orderId });
            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 2, Message = "未找到订单" });
            }
            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.异常)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 3, Message = "只有异常状态下的订单才能取消" });
            }
            var flowInfo = orderInfo.OrderDetails.FirstOrDefault(t => t.FlowId == flowId);
            if (flowInfo == null)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 4, Message = "参数flowId错误" });
            }
            if (flowInfo.OrderStatus != AgentEnums.OrderDetailStatus.无货异常)
            {
                return Json(new ApiResult<int>(2) { ErrorCode = 5, Message = "只有无货异常的订单才能取消" });
            }
            var result = OrderService.Instance.UpdateOrderDetail(new { orderStatus = AgentEnums.OrderDetailStatus.已取消 }, new { flowId });
            if (result)
            {
                OrderLogsService.Instance.CreateLog(new OrderLogs
                {
                    OrderId = orderId,
                    UserId = user.UserId,
                    UserName = user.NickName,
                    Title = "取消订单",
                    Content = user.UserName + "取消了子订单,商品编号:" + flowInfo.ProductNo,
                    CreateDate = DateTime.Now
                });
            }
            return Json(new ApiResult<bool>(result));
        }
        #endregion
    }
}