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
        private readonly ProductStockService _productStockService = new ProductStockService();
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderList()
        {
            return View();
        }

        public ActionResult AddOrder()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult ProductStocks()
        {
            return View();
        }

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

            return Json(new ApiResult<bool>(result) {Message = result ? "操作成功" : "操作失败"});
        }


        public JsonResult GetOrderList(UserInfo user, int orderStatus, string beginDateStr, string endDateStr,
            string keyWords, int page = 1,
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

            return Json(new ApiPageList<OrderMaster>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        public JsonResult SearchProduct(string keyWords)
        {
            var list = _productService.SearchProduct(keyWords);

            return Json(list);
        }


        public JsonResult GetProductStock(int productId)
        {
            var list =
                _productStockService.GetList(new {productId, status = 0})
                    .OrderBy(t => t.SkuName)
                    .Where(t => t.StockNum > 0);

            return Json(list);
        }
    }
}