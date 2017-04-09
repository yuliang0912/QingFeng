using System.Linq;
using QingFeng.Business;
using QingFeng.Models;
using QingFeng.WebArea.FormsAuth;
using System.Web.Mvc;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.WebArea.Controllers;
using static QingFeng.Common.AgentEnums;
using System;
using System.Collections.Generic;

namespace QingFeng.WebArea.Fillter
{
    public class MenuAuthorize : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        readonly SubMenuEnum[] _subMenus;
        protected UserInfo CurrentUser;


        public MenuAuthorize(params SubMenuEnum[] subMenus)
        {
            _subMenus = subMenus;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userId = FormsAuthenticationService.Instance.UserId;

            CurrentUser = string.IsNullOrEmpty(userId) ? null : UserService.Instance.GetUserInfo(new {userId});

            if (CurrentUser != null && CurrentUser.UserRole == UserRole.Administrator)
            {
                var menuList = new List<int>();
                foreach (var item in Enum.GetValues(typeof(SubMenuEnum)))
                {
                    menuList.Add(item.GetHashCode());
                }
                CurrentUser.UserMenus = string.Join(",", menuList);
            }

            CurrentUser.UserMenus =
                    "0,101,102,103,104,201,202,203,204,205,206,207,208,209,210,301,302,303,304,305,401,402,403,404,501,502,503,504,505,506,507,508,509,510,511,512,513,514,515,516,517,518,601,602,603";

            if (CurrentUser == null || !CurrentUser.AllUserMenus.Any(t => _subMenus.Contains(t)))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new CustomJsonResult()
                    {
                        Data = new ApiResult(RetEum.AuthenticationFailure, -1, "没有操作权限")
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/home/login");
                }
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