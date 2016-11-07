using System;
using System.Net;

namespace StoreSaas.Common.ApiCore.Excepition
{
    /// <summary>
    /// API异常基类
    /// </summary>
    public class ApiException : Exception
    {
        public ApiException(RetEum ret, int errCode, string message, HttpStatusCode status = HttpStatusCode.OK)
        {
            this.Ret = ret;
            this.ErrCode = errCode;
            this.Message = message;
            this.HttpStatus = status;
        }

        /// <summary>
        /// 服务器状态码
        /// </summary>
        public RetEum Ret { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public new string Message { get; set; }

        /// <summary>
        /// Http状态码
        /// </summary>
        public HttpStatusCode HttpStatus { get; set; }
    }

    /// <summary>
    /// 参数异常
    /// </summary>
    public class ApiArgumentException : ApiException
    {
        public ApiArgumentException(string message, int errCode) : base(RetEum.Success, errCode, message) { }
    }

    /// <summary>
    /// 鉴权失败
    /// </summary>
    public class ApiAuthenticationException : ApiException
    {
        public ApiAuthenticationException(string message = "鉴权失败", int errCode = 1) : base(RetEum.AuthenticationFailure, errCode, message) { }
    }
}
