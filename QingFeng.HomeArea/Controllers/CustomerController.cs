using System;
using System.Text;
using System.Web.Mvc;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;

namespace QingFeng.WebArea.Controllers
{
    public class CustomerController : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }

    public class CustomJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data is ApiResult)
            {
                response.Write(JsonHelper.Encode(Data));
            }
            else
            {
                response.Write(JsonHelper.Encode(new ApiResult<object>(Data)));
            }
        }
    }
}