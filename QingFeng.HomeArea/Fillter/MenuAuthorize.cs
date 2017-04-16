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
                return;
            }

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