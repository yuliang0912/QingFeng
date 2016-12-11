using System.Linq;
using QingFeng.Business;
using QingFeng.Models;
using QingFeng.WebArea.FormsAuth;
using System.Web.Mvc;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.WebArea.Controllers;

namespace QingFeng.WebArea.Fillter
{
    public class AdminAuthorize : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        readonly Common.AgentEnums.UserRole _allowRole;

        public AdminAuthorize(Common.AgentEnums.UserRole allowRole)
        {
            _allowRole = allowRole;
            //_allowRole = Common.AgentEnums.UserRole.AllUser;
        }

        protected UserInfo CurrentUser;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userId = FormsAuthenticationService.Instance.UserId;

            CurrentUser = string.IsNullOrEmpty(userId) ? null : new UserService().GetUserInfo(new {userId});

            //CurrentUser = new UserService().GetUserInfo(new {userName = "admin"});

            if (CurrentUser != null &&
                (CurrentUser.UserRole == _allowRole || _allowRole == Common.AgentEnums.UserRole.AllUser))
            {
                CurrentUser.StoreList = CurrentUser.StoreList.Where(t => t.Status == 0).ToList();
                return;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new CustomJsonResult()
                {
                    Data = new ApiResult(RetEum.AuthenticationFailure, -1, "未检测到登陆用户")
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("/home/login");
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (CurrentUser != null)
            {
                filterContext.Controller.ViewBag.User = CurrentUser;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("user"))
            {
                filterContext.ActionParameters["user"] = CurrentUser;
            }
        }
    }
}