using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using StoreSaas.Common.ApiCore.Excepition;

namespace StoreSaas.Common.ApiCore.Filter
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);

            var exception = actionExecutedContext.Exception;

            if (exception == null) return;

            var apiException = exception as ApiException ??
                               new ApiException(RetEum.ServerError, (int) RetEum.ApplicationError, exception.Message,
                                   HttpStatusCode.InternalServerError);

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(apiException.HttpStatus,
                apiException);
        }
    }
}
