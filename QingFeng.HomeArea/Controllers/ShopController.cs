﻿using QingFeng.Business;
using System.Web.Mvc;
using System.Linq;

namespace QingFeng.WebArea.Controllers
{
    public class ShopController : CustomerController
    {
        // GET: Shop
        public ActionResult Index()
        {
            var stopList = StoreService.Instance.GetList(null).OrderBy(t => t.CreateDate).ToList();

            return View(stopList);
        }

        public ActionResult Edit()
        {
            return null;
        }


        #region 更新状态
        [HttpGet]

        public JsonResult UpdateStatus(int storeId, int status)
        {
            var result = StoreService.Instance.UpdateStoreStatus(storeId, status);

            return Json(result);
        }
        #endregion
    }
}