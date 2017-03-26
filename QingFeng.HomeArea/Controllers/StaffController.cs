using System;
using System.Linq;
using QingFeng.Common;
using QingFeng.WebArea.Fillter;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Models;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Common.ApiCore;

namespace QingFeng.WebArea.Controllers
{
    public class StaffController : CustomerController
    {
        // GET: Staff
        [AdminAuthorize(AgentEnums.SubMenuEnum.员工列表)]
        public ActionResult Index(string keyWords = "")
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.Staff, keyWords).ToList();

            ViewBag.keyWords = keyWords;

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.编辑店铺)]
        public ActionResult Edit(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Content("参数错误");
            }

            return View(userInfo);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.添加店铺)]
        public ActionResult Add()
        {
            return View();
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商列表)]
        public ActionResult Agent()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.重置代理商密码)]
        public ActionResult ReSetAgentPwd()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.删除代理商)]
        public ActionResult SetAgentLogin()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        #region Ajax

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult Edit()
        {
            return Json(null);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult AddStaff(UserInfo model)
        {
            model.CreateDate = DateTime.Now;
            model.Status = 0;
            model.UserRole = AgentEnums.UserRole.Staff;
            model.UserMenus = "210";

            var result = UserService.Instance.RegisterUser(model);
            return Json(result);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult ProhibitLogin(UserInfo user, int userId, int status)
        {
            if (user.UserId == userId)
            {
                return Json(new ApiResult<int>(2) { Ret = RetEum.ApplicationError, Message = "不能更新自己的状态" });
            }

            var result = UserService.Instance.UpdateUserInfo(new UserInfo() { UserId = userId, Status = status });

            return Json(new ApiResult<bool>(result));
        }

        #endregion
    }
}