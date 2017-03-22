using System;
using System.Linq;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.AllUser)]
    public class AgentController : CustomerController
    {
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderList(UserInfo user, int storeId = 0, int orderStatus = 0, string beginDateStr = "",
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

            int totalItem;
            var list = OrderService.Instance.SearchOrderList(user.UserId, storeId, orderStatus, beginDate, endDate, keyWords,
                page,
                pageSize, out totalItem);

            ViewBag.ProductBase = ProductService.Instance.GetProductBaseList(
                list.SelectMany(t => t.OrderDetails).Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = ProductService.Instance.GetProduct(
                list.SelectMany(t => t.OrderDetails).Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

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

        public ActionResult AddOrder()
        {
            return View();
        }

        public ActionResult Products(string keyWords = "", int categoryId = 0, int page = 1, int pageSize = 10)
        {
            int totalItem;

            var list = ProductService.Instance.SearchBaseProduct(0, 0, categoryId, keyWords, 0, page, pageSize, out totalItem);

            ViewBag.categoryId = categoryId;
            ViewBag.keyWords = keyWords;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        public ActionResult ProductStocks(int categoryId = 0, string baseNo = "")
        {
            if (string.IsNullOrEmpty(baseNo))
            {
                return View(new ProductBase());
            }

            var model = ProductService.Instance.GetProductBase(baseNo);
            if (model == null)
            {
                return Content("<script>alert('未找到指定的产品号');location.href='/agent/ProductStocks';</script>");
            }

            model.SubProduct = ProductService.Instance.GetProductByBaseId(model.BaseId);

            var productStockList = ProductStockService.Instance.GetList(new {model.BaseId});

            var productStocks = productStockList
                .GroupBy(t => t.ProductId)
                .ToDictionary(t => t.Key, t => t);

            ViewBag.categoryId = categoryId;
            ViewBag.allSkus = productStockList.GroupBy(t => t.SkuId).ToDictionary(t => t.Key, t => t.First().SkuName);

            model.SubProduct.ToList().ForEach(t =>
            {
                if (productStocks.ContainsKey(t.ProductId))
                {
                    t.ProductStocks = productStocks[t.ProductId].ToList();
                }
            });

            return View(model);
        }

        public ActionResult OrderDetail(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new {orderId, user.UserId});
            if (order == null)
            {
                return Content("未找到指定的订单");
            }
            ViewBag.ProductBase = ProductService.Instance.GetProductBaseList(order.OrderDetails.Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = ProductService.Instance.GetProduct(order.OrderDetails.Select(t => t.ProductId).ToArray())
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

        #region ajax

        [HttpPost]
        public JsonResult CreateOrder(UserInfo user, OrderMaster order)
        {
            if (string.IsNullOrWhiteSpace(order?.OrderNo))
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据错误"});
            }
            if (OrderService.Instance.IsExists(order.OrderNo))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "订单号已经存在"});
            }
            if (user.StoreList.All(t => t.StoreId != order.StoreId))
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "店铺信息错误"});
            }

            foreach (var item in order.OrderDetails)
            {
                var stock = ProductStockService.Instance.Get(new {item.ProductId, item.SkuId});
                if (stock == null || stock.StockNum < 1 || item.Quantity > stock.StockNum)
                {
                    return Json(new ApiResult<int>(5) {Ret = RetEum.ApplicationError, Message = "库存不足"});
                }
            }

            order.UserId = user.UserId;

            var result = OrderService.Instance.CreateOrder(user, order, order.OrderDetails.ToList());

            return Json(new ApiResult<bool>(result) {Message = result ? order.OrderId.ToString() : "操作失败"});
        }


        public JsonResult SearchProduct(string keyWords)
        {
            var list = ProductService.Instance.SearchProduct(keyWords);

            var baseList = ProductService.Instance.GetProductBaseList(list.Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            return Json(list.Select(x => new
            {
                baseId = x.BaseId,
                baseNo = baseList[x.BaseId].BaseNo,
                productId = x.ProductId,
                baseName = x.BaseName,
                productName = x.ProductName,
                productNo = x.ProductNo,
                originalPrice = x.OriginalPrice,
                actualPrice = x.ActualPrice,
                categoryName = baseList[x.BaseId].CategoryId.ToString()
            }));
        }


        public JsonResult GetProductStock(int productId)
        {
            var list =
                ProductStockService.Instance.GetList(new {productId})
                    .OrderBy(t => t.SkuName)
                    .Where(t => t.StockNum > 0);

            return Json(list);
        }


        public JsonResult SearchBaseProductNo(string keyWords, int categoryId = 0)
        {
            if (string.IsNullOrWhiteSpace(keyWords))
            {
                return Json(Enumerable.Empty<object>());
            }

            var list = ProductService.Instance.SearchBaseProduct(categoryId, 0, keyWords).Select(t => new
            {
                baseId = t.BaseId,
                baseNo = t.BaseNo
            });
            return Json(list);
        }

        [HttpPost]
        public JsonResult PayOrder(UserInfo user, long orderId)
        {
            var order = OrderService.Instance.Get(new {orderId, userId = user.UserId});

            if (order == null)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "未找到有效订单"});
            }

            if (order.OrderStatus != AgentEnums.MasterOrderStatus.待支付)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "只有待支付状态的订单才能继续支付"});
            }

            var result = OrderService.Instance.UpdateOrder(new {orderStatus = AgentEnums.MasterOrderStatus.待发货.GetHashCode()},
                new {orderId});

            if (result)
            {
                OrderLogsService.Instance.CreateLog(new OrderLogs()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Title = "支付订单",
                    Content = user.UserName + "已支付了订单,等待后台管理员审核",
                    CreateDate = DateTime.Now,
                    OrderId = orderId
                });
            }

            return Json(result);
        }

        #endregion
    }
}