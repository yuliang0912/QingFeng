using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.AllUser)]
    public class AdminController : CustomerController
    {
        private readonly UserService _userService = new UserService();
        private readonly OrderService _orderService = new OrderService();
        private readonly SkuItemService _skuItemService = new SkuItemService();
        private readonly ProductService _productService = new ProductService();
        private readonly ProductStockService _productStockService = new ProductStockService();


        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult UserList(int page = 1, int pageSize = 20)
        {
            int totalItem;
            var list = _userService.GetPageList(new {userRole = AgentEnums.UserRole.StoreUser.GetHashCode()}, page,
                pageSize, out totalItem);

            return View(new ApiPageList<UserInfo>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }


        public ActionResult EditUser(int userId)
        {
            var userInfo = _userService.GetUserInfo(new {userId, userRole = AgentEnums.UserRole.StoreUser.GetHashCode()});

            if (userInfo == null)
            {
                return Content("未找到有效用户");
            }

            return View(userInfo);
        }

        public ActionResult EditBaseProduct(int baseId)
        {
            var model = _productService.GetProductBase(baseId);

            if (model == null)
            {
                return Content("未找到有效产品");
            }

            return View(model);
        }


        public ActionResult AddBaseProduct()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult BaseProducts(int page = 1, int pageSize = 20)
        {
            int totalItem;
            var list = _productService.SearchBaseProduct(string.Empty, 0, page, pageSize, out totalItem);

            return View(new ApiPageList<ProductBase>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        public ActionResult ColorList()
        {
            var list = _skuItemService.GetList(AgentEnums.SkuType.Color);
            return View(list);
        }

        public ActionResult SizeList()
        {
            var list = _skuItemService.GetList(AgentEnums.SkuType.Size).OrderBy(t => t.SkuName);
            return View(list);
        }


        #region Ajax

        [HttpPost]
        public JsonResult AddStoreUser(UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo.UserName) || string.IsNullOrWhiteSpace(userInfo.PassWord))
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "用户名和密码不能为空"});
            }

            var result = _userService.RegisterUser(userInfo);

            return Json(new ApiResult<int>(result));
        }

        /// <summary>
        /// 添加基础产品信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateBaseProduct(UserInfo user, ProductBase model)
        {
            if (model == null)
            {
                return Json(new ApiResult<int>(2) {Message = "未接收到数据"});
            }

            if (string.IsNullOrWhiteSpace(model.BaseNo) || string.IsNullOrWhiteSpace(model.BaseName))
            {
                return Json(new ApiResult<int>(3) {Message = "货名和货号不能为空"});
            }

            if (_productService.GetProductBase(model.BaseNo) != null)
            {
                return Json(new ApiResult<int>(4) {Message = "当前货号已经存在"});
            }

            model.CreateUserId = user.UserId;
            var result = _productService.CreateBaseProduct(model);
            return Json(new ApiResult<bool>(result));
        }


        [HttpPost]
        public JsonResult EditBaseProduct(UserInfo user, ProductBase model)
        {
            if (model == null)
            {
                return Json(new ApiResult<int>(2) {Message = "未接收到数据"});
            }

            var baseProduct = _productService.GetProductBase(model.BaseId);
            if (baseProduct == null)
            {
                return Json(new ApiResult<int>(5) {Message = "参数错误,未找到制定商品"});
            }

            if (string.IsNullOrWhiteSpace(model.BaseName))
            {
                return Json(new ApiResult<int>(3) {Message = "货名和货号不能为空"});
            }

            var result = _productService.UpdateProductBaseInfo(new
            {
                model.BaseName,
                model.OriginalPrice,
                model.ActualPrice,
                categoryId = model.CategoryId.GetHashCode()
            }, new {baseProduct.BaseId});

            return Json(new ApiResult<bool>(result));
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

        [HttpPost]
        public JsonResult UpdateUserInfo(UserInfo userInfo)
        {
            return Json(_userService.UpdateUserInfo(userInfo));
        }

        public JsonResult DelOrRecoveryStatus(UserInfo user, int userId)
        {
            return Json(_userService.DelOrRecoveryStatus(userId));
        }

        public JsonResult UpdatePassWord(UserInfo user, int userId, string passWord)
        {
            if (string.IsNullOrWhiteSpace(passWord))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "密码不能为空"});
            }

            return Json(_userService.UpdatePassWord(userId, passWord));
        }

        [HttpPost]
        public JsonResult SendDeliverGoods(long orderId, List<int> flowIds, LogisticsInfo model)
        {
            var orderInfo = _orderService.Get(new {orderId});

            if (orderInfo == null)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据接收失败"});
            }

            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.已支付 &&
                orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.待发货 &&
                orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.进行中)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "订单状态错误"});
            }

            if (orderInfo.OrderDetails.Count(t => flowIds.Contains(t.FlowId)) == flowIds.Count)
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "子订单数据错误"});
            }

            if (
                orderInfo.OrderDetails.Where(t => flowIds.Contains(t.FlowId))
                    .Any(t => t.OrderSatus == AgentEnums.OrderDetailStatus.已发货))
            {
                return Json(new ApiResult<int>(5) {Ret = RetEum.ApplicationError, Message = "存在重复发货情况"});
            }

            if (
                orderInfo.OrderDetails.Where(t => flowIds.Contains(t.FlowId))
                    .Any(t => t.OrderSatus == AgentEnums.OrderDetailStatus.已取消))
            {
                return Json(new ApiResult<int>(6) {Ret = RetEum.ApplicationError, Message = "已取消的商品,不能发货"});
            }

            var result = _orderService.SendDeliverGoods(orderInfo, flowIds, model);

            return Json(result);
        }

        public JsonResult UpdateSkuStatus(int skuId, int status)
        {
            status = status == 0 ? 0 : 1;

            var result = _skuItemService.Update(new {status}, new {skuId});

            return Json(result);
        }

        [HttpPost]
        public JsonResult AddSku(SkuItem sku)
        {
            if (sku == null)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "未接收到有效数据"});
            }
            if (string.IsNullOrWhiteSpace(sku.SkuName))
            {
                return Json(new ApiResult<int>(4) {Ret = RetEum.ApplicationError, Message = "名称不能为空"});
            }
            sku.SkuImgUrl = string.Empty;
            var result = _skuItemService.AddSkuItem(sku);
            return Json(result);
        }

        #endregion
    }
}