using System;
using System.Collections.Generic;
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
    [AdminAuthorize(AgentEnums.UserRole.Administrator)]
    public class AdminController : CustomerController
    {
        private readonly UserService _userService = new UserService();
        private readonly OrderService _orderService = new OrderService();
        private readonly ProductService _productService = new ProductService();
        private readonly ProductStockService _productStockService = new ProductStockService();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AddStoreUser(UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo.UserName) || string.IsNullOrWhiteSpace(userInfo.PassWord))
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "用户名和密码不能为空"});
            }

            if (_userService.Count(new {userName = userInfo.UserName}) > 0)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "用户名已经存在"});
            }

            var result = _userService.RegisterUser(userInfo);

            return Json(new ApiResult<int>(result));
        }


        [HttpPost]
        public JsonResult SendDeliverGoods(long orderId, List<int> flowIds, LogisticsInfo model)
        {
            var orderInfo = _orderService.Get(new {orderId});

            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据接收失败"});
            }

            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.AlreadyPay &&
                orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.WaitDeliverGoods &&
                orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.Doing)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "订单状态错误"});
            }

            if (orderInfo.OrderDetails.Count(t => flowIds.Contains(t.FlowId)) == flowIds.Count)
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "子订单数据错误"});
            }

            if (
                orderInfo.OrderDetails.Where(t => flowIds.Contains(t.FlowId))
                    .Any(t => t.OrderSatus == AgentEnums.OrderDetailStatus.HasDeliverGoods))
            {
                return Json(new ApiResult<int>(5) {Ret = RetEum.ApplicationError, Message = "存在重复发货情况"});
            }

            if (
                orderInfo.OrderDetails.Where(t => flowIds.Contains(t.FlowId))
                    .Any(t => t.OrderSatus == AgentEnums.OrderDetailStatus.Canceled))
            {
                return Json(new ApiResult<int>(6) {Ret = RetEum.ApplicationError, Message = "已取消的商品,不能发货"});
            }

            var result = _orderService.SendDeliverGoods(orderInfo, flowIds, model);

            return Json(result);
        }


        public JsonResult GetOrderList(int orderStatus, string beginDateStr, string endDateStr,
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
            var list = _orderService.SearchOrderList(0, orderStatus, beginDate, endDate, keyWords,
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
    }
}