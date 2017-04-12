using System;
using System.Linq;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, AdminAuthorize(AgentEnums.SubMenuEnum.设置权限)]
        public ActionResult SetRole(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId});

            if (userInfo == null || userInfo.UserRole == AgentEnums.UserRole.StoreUser)
            {
                return Content("参数错误");
            }

            ViewBag.userId = userId;
            ViewBag.userRoles = string.Join(",", userInfo.AllUserMenus.Select(t => t.GetHashCode()));

            var allMenus = MenuService.Instance.GetList();

            return View(allMenus);
        }


        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.设置权限)]
        public JsonResult SetRole()
        {
            var userId = Convert.ToInt32(Request.Form["userId"] ?? string.Empty);
            var userRoles = Request.Form["userRoles"] ?? string.Empty;

            var userInfo = UserService.Instance.GetUserInfo(new {userId});
            if (userInfo == null || userInfo.UserRole != AgentEnums.UserRole.Staff)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "参数错误"});
            }

            var result = UserService.Instance.Update(new
            {
                userMenus = userRoles
            }, new {userId});

            return Json(result);
        }
    }
}