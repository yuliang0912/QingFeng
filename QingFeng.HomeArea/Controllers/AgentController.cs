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
    [AdminAuthorize(AgentEnums.UserRole.StoreUser)]
    public class AgentController : CustomerController
    {

        private readonly OrderService _orderService = new OrderService();
        private readonly ProductService _productService = new ProductService();
        private readonly OrderLogsService _orderLogsService = new OrderLogsService();
        private readonly LogisticsService _logisticsService = new LogisticsService();
        private readonly ProductStockService _productStockService = new ProductStockService();
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderList(UserInfo user, int orderStatus = 0, string beginDateStr = "",
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
            var list = _orderService.SearchOrderList(user.StoreInfo.StoreId, orderStatus, beginDate, endDate, keyWords,
                page,
                pageSize, out totalItem);

            ViewBag.ProductBase = _productService.GetProductBaseList(
                    list.SelectMany(t => t.OrderDetails).Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = _productService.GetProduct(
                    list.SelectMany(t => t.OrderDetails).Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            ViewBag.beginDateStr = beginDateStr;
            ViewBag.endDateStr = endDateStr;
            ViewBag.keyWords = keyWords;

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

        public ActionResult Products(string keyWords = "", int categoryId = 0, int page = 1, int pageSize = 20)
        {
            int totalItem;

            var list = _productService.SearchBaseProduct(keyWords, categoryId, page, pageSize, out totalItem);

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

            var model = _productService.GetProductBase(baseNo);
            if (model == null)
            {
                return Content("<script>alert('未找到指定的产品号');location.href='/agent/ProductStocks';</script>");
            }

            model.SubProduct = _productService.GetProductByBaseId(model.BaseId);

            var productStockList = _productStockService.GetList(new {model.BaseId});

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
            var order = _orderService.Get(new {orderId, user.StoreInfo.StoreId});
            if (order == null)
            {
                return Content("未找到指定的订单");
            }
            ViewBag.ProductBase = _productService.GetProductBaseList(order.OrderDetails.Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            ViewBag.PorductList = _productService.GetProduct(order.OrderDetails.Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            ViewBag.Logistics = _logisticsService.GetLogistics(orderId);

            ViewBag.OrderLogs = _orderLogsService.GetList(orderId);

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
            if (_orderService.IsExists(order.OrderNo))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "订单号已经存在"});
            }

            order.UserId = user.UserId;
            order.StoreId = user.StoreInfo.StoreId;

            var result = _orderService.CreateOrder(user, order, order.OrderDetails.ToList());

            return Json(new ApiResult<bool>(result) {Message = result ? order.OrderId.ToString() : "操作失败"});
        }


        public JsonResult SearchProduct(string keyWords)
        {
            var list = _productService.SearchProduct(keyWords);

            var baseList = _productService.GetProductBaseList(list.Select(t => t.BaseId).ToArray())
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
                _productStockService.GetList(new {productId})
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

            var list = _productService.SearchBaseProduct(categoryId, keyWords).Select(t => new
            {
                baseId = t.BaseId,
                baseNo = t.BaseNo
            });
            return Json(list);
        }

        #endregion
    }
}