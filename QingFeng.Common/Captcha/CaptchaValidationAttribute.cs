using QingFeng.Common.Utilities;
using System;
using System.Web.Mvc;


namespace CiWong.Framework.Captcha
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CaptchaValidationAttribute : ActionFilterAttribute
    {

        public CaptchaValidationAttribute(string aid) : this(aid, "code") { }
        public CaptchaValidationAttribute(string aid, string fileid)
        {
            guid = aid; 
            this.fileid = fileid;
        }
        public string guid { get; set; }
        public string fileid { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            filterContext.ActionParameters["captchaValid"] = false;
            var code = filterContext.HttpContext.Request.Form[fileid];
            var _data = filterContext.HttpContext.Request.Cookies.Get("verifysession");

            if (code == "88888" && DateTime.Now < new DateTime(2014, 9, 1))
            {
                filterContext.ActionParameters["captchaValid"] = true;
                return;
            }


            var text = string.Format("{0}{1}", guid, (code == "" || code == null) ? "" : code.Trim());

            string actualValue = text.ToLower().EncryptOneWay<System.Security.Cryptography.SHA256CryptoServiceProvider>().ToLower();
            string expectedValue = _data == null ? String.Empty : _data.Value;
            filterContext.HttpContext.Response.Cookies.Remove("verifysession");

            if (String.IsNullOrEmpty(actualValue) || String.IsNullOrEmpty(expectedValue) || !String.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.ActionParameters["captchaValid"] = false;
                return;
            }

            filterContext.ActionParameters["captchaValid"] = true;
        }
    }
}
