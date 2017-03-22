using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;
using QingFeng.WebArea.FormsAuth;

namespace QingFeng.WebArea.Controllers
{
    public class HomeController : CustomerController
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthenticationWrapper.Instance.SignOut();
            return Redirect("/home/login");
        }

        public JsonResult LoginCheck(string loginName, string passWord)
        {
            bool isPass;
            var userInfo = UserService.Instance.Login(loginName, passWord, out isPass);
            if (userInfo != null && userInfo.Status != 0)
            {
                return Json(new {isPass = 2, userRole = userInfo?.UserRole}, JsonRequestBehavior.AllowGet);
            }
            if (isPass)
            {
                FormsAuthenticationWrapper.Instance.SetAuthCookie(userInfo.UserId.ToString(), false);
            }
            return Json(new {isPass, userRole = userInfo?.UserRole}, JsonRequestBehavior.AllowGet);
        }


        [AdminAuthorize]
        public ActionResult UpdateUserStatus(UserInfo user, int userId, int status)
        {
            if (user.UserId == userId)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "不能更新自己的状态"});
            }

            var result = UserService.Instance.UpdateUserInfo(new UserInfo() {UserId = userId, Status = status});

            return Json(new ApiResult<bool>(result));
        }


        [AdminAuthorize, HttpGet]
        public ActionResult UpdatePassWord(UserInfo user, string oldPwd, string newPwd)
        {
            if (string.IsNullOrWhiteSpace(oldPwd) || string.IsNullOrWhiteSpace(newPwd))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "密码不能为空"});
            }

            return Json(UserService.Instance.UpdatePassWord(user, oldPwd, newPwd));
        }
    }
}