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
        private readonly UserService _userService = new UserService();
        // GET: Home
        [AdminAuthorize(AgentEnums.UserRole.AllUser)]
        public ActionResult Index(UserInfo user)
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

        public JsonResult LoginCheck(string loginName, string passWord)
        {
            bool isPass;
            var userInfo = _userService.Login(loginName, passWord, out isPass);
            if (isPass)
            {
                FormsAuthenticationWrapper.Instance.SetAuthCookie(userInfo.UserId.ToString(), false);
            }
            return Json(new {isPass, userRole = userInfo?.UserRole}, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetUserList(int page = 1, int pageSize = 10)
        //{
        //    int totalItem;
        //    var list = _userService.GetPageList(page, pageSize, out totalItem);

        //    return Json(new ApiPageList<UserInfo>()
        //    {
        //        Page = page,
        //        PageSize = pageSize,
        //        TotalCount = totalItem,
        //        PageList = list
        //    });
        //}


        [AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public ActionResult UpdateUserStatus(UserInfo user, int userId, int status)
        {
            if (user.UserId == userId)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "不能更新自己的状态"});
            }

            var result = _userService.UpdateUserInfo(new UserInfo() {UserId = userId, Status = status});

            return Json(new ApiResult<bool>(result));
        }


        [AdminAuthorize(AgentEnums.UserRole.AllUser), HttpGet]
        public ActionResult UpdatePassWord(UserInfo user, string oldPwd, string newPwd)
        {
            if (string.IsNullOrWhiteSpace(oldPwd) || string.IsNullOrWhiteSpace(newPwd))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "密码不能为空"});
            }

            return Json(_userService.UpdatePassWord(user, oldPwd, newPwd));
        }
    }
}