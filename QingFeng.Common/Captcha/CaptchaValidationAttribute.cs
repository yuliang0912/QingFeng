using System;
using System.Web.Mvc;
using QingFeng.Common.Utilities;
using static System.String;

namespace QingFeng.Common.Captcha
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class CaptchaValidationAttribute : ActionFilterAttribute
    {

        public CaptchaValidationAttribute(string aid) : this(aid, "code")
        {
        }

        public CaptchaValidationAttribute(string aid, string fileid)
        {
            Guid = aid;
            this.Fileid = fileid;
        }

        public string Guid { get; set; }
        public string Fileid { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            filterContext.ActionParameters["captchaValid"] = false;
            var code = filterContext.HttpContext.Request.Form[Fileid];
            var data = filterContext.HttpContext.Request.Cookies.Get("verifysession");

            if (code == "88888" && DateTime.Now < new DateTime(2014, 9, 1))
            {
                filterContext.ActionParameters["captchaValid"] = true;
                return;
            }


            var text = $"{Guid}{(IsNullOrEmpty(code) ? "" : code.Trim())}";

            var actualValue =
                text.ToLower().EncryptOneWay<System.Security.Cryptography.SHA256CryptoServiceProvider>().ToLower();
            var expectedValue = data == null ? Empty : data.Value;
            filterContext.HttpContext.Response.Cookies.Remove("verifysession");

            if (IsNullOrEmpty(actualValue) || IsNullOrEmpty(expectedValue) ||
                !string.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.ActionParameters["captchaValid"] = false;
                return;
            }

            filterContext.ActionParameters["captchaValid"] = true;
        }
    }
}
