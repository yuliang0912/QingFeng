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
    public class HomeController : Controller
    {
        private readonly UserService _userService = new UserService();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult LoginCheck(string loginName, string passWord)
        {
            bool isPass;
            var userInfo = _userService.Login(loginName, passWord, out isPass);
            if (isPass)
            {
                FormsAuthenticationWrapper.Instance.SetAuthCookie(userInfo.UserId.ToString(), false);
            }
            return Json(isPass);
        }

        public JsonResult GetUserList(int page = 1, int pageSize = 10)
        {
            int totalItem;
            var list = _userService.GetPageList(page, pageSize, out totalItem);

            return Json(new ApiPageList<UserInfo>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        [AdminAuthorize(AgentEnums.UserRole.Administrator)]
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
    }
}