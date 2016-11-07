using QingFeng.Business;
using QingFeng.Models;
using QingFeng.WebArea.FormsAuth;
using StoreSaas.AdminArea.Code;
using StoreSaas.Common.ApiCore;
using StoreSaas.Common.ApiCore.Result;
using System.Web.Mvc;

namespace QingFeng.WebArea.Fillter
{
    public class AdminAuthorize : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        Common.AgentEnums.UserRole allowRole;
        public AdminAuthorize(Common.AgentEnums.UserRole _allowRole)
        {
            allowRole = _allowRole;
        }
        protected UserInfo currentUser;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userId = FormsAuthenticationService.Instance.UserId;

            currentUser = string.IsNullOrEmpty(userId) ? null : new UserService().GetUserInfo(new { userId });

            if (currentUser != null && currentUser.UserRole == allowRole)
            {
                return;
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
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
            if (currentUser != null)
            {
                filterContext.Controller.ViewBag.User = currentUser;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("user"))
            {
                filterContext.ActionParameters["user"] = currentUser;
            }
        }
    }
}