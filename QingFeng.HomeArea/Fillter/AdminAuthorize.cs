using QingFeng.Business;
using QingFeng.Models;
using QingFeng.WebArea.FormsAuth;
using StoreSaas.AdminArea.Code;
using StoreSaas.Common.ApiCore;
using StoreSaas.Common.ApiCore.Result;
using System;
using System.Web.Mvc;

namespace QingFeng.WebArea.Fillter
{
    public class AdminAuthorize : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        protected UserInfo currentUser;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userId = FormsAuthenticationService.Instance.UserId;

            currentUser = string.IsNullOrEmpty(userId) ? null : new UserService().GetUserInfo(new { userId });

            if (currentUser != null) return;

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
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}