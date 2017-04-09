using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Common.Captcha;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;
using QingFeng.WebArea.FormsAuth;

namespace QingFeng.WebArea.Controllers
{
    public class HomeController : CustomerController
    {
        public ActionResult Index()
        {
            return RedirectToAction("WelCome");
        }

        [AdminAuthorize]
        public ActionResult WelCome()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthenticationWrapper.Instance.SignOut();
            return Redirect("/home/login");
        }

        [AdminAuthorize]
        public ActionResult ResetPassWord()
        {
            return View();
        }

        [CaptchaValidation("login", "captcha")]
        public ActionResult CheckLogin(bool captchaValid)
        {
            if (!captchaValid)
            {
                ViewBag.message = "验证码错误";
                return View("Login");
            }

            bool isPass;
            var userName = Request.Form["username"] ?? string.Empty;
            var password = Request.Form["password"] ?? string.Empty;

            var ip = HttpContext.Request.UserHostAddress;
            var userInfo = UserService.Instance.Login(userName, password, ip, out isPass);

            if (userInfo == null || !isPass)
            {
                ViewBag.message = "用户名或密码错误";
                return View("Login");
            }
            if (userInfo.Status != 0)
            {
                ViewBag.message = "此用户已被禁止登陆";
                return View("Login");
            }
            FormsAuthenticationWrapper.Instance.SetAuthCookie(userInfo.UserId.ToString(), false);
            return Redirect("/home/welcome");
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