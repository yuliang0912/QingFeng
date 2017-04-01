using System;
using QingFeng.Business;
using System.Web.Mvc;
using System.Linq;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    public class ShopController : CustomerController
    {
        // GET: Shop
        [MenuAuthorize(AgentEnums.SubMenuEnum.店铺管理)]
        public ActionResult Index()
        {
            var stopList = StoreService.Instance.GetList(null).OrderBy(t => t.CreateDate).ToList();

            ViewBag.list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(stopList);
        }


        [HttpPost, MenuAuthorize(AgentEnums.SubMenuEnum.添加店铺)]
        public ActionResult Add()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [MenuAuthorize(AgentEnums.SubMenuEnum.编辑店铺)]
        public ActionResult Edit()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }


        #region 更新状态

        [HttpGet, MenuAuthorize(AgentEnums.SubMenuEnum.删除店铺)]

        public JsonResult UpdateStatus(int storeId, int status)
        {
            var result = StoreService.Instance.UpdateStoreStatus(storeId, status);

            return Json(result);
        }

        [HttpPost, MenuAuthorize(AgentEnums.SubMenuEnum.编辑店铺)]
        public JsonResult EditShop(StoreInfo model)
        {
            var modelInfo = StoreService.Instance.GetStoreInfo(new {model.StoreId});
            if (modelInfo == null)
            {
                return Json(new ApiResult<int>(2) {ErrorCode = 1, Message = "参数错误"});
            }
            return Json(null);
        }


        [HttpPost, MenuAuthorize(AgentEnums.SubMenuEnum.添加店铺)]
        public JsonResult AddShop(StoreInfo model)
        {
            model.CreateDate = DateTime.Now;
            model.Status = 0;
            model.IsSelfSupport = model.MasterUserId == 0 ? 1 : 0;
            model.StoreCode = model.StoreCode ?? string.Empty;
            model.StoreName = model.StoreName ?? string.Empty;

            var result = StoreService.Instance.CreateStore(model);

            return Json(result);
        }

        #endregion
    }
}