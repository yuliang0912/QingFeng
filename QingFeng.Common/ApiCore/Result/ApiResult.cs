using Newtonsoft.Json;

namespace StoreSaas.Common.ApiCore.Result
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult<T> : ApiResult
    {
        public ApiResult(T data)
        {
            this.Data = data;
        }

        public ApiResult()
        {
        }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class ApiResult
    {
        public ApiResult(RetEum ret, int errorCode, string message)
        {
            this.Ret = ret;
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        public ApiResult() : this(RetEum.Success, 0, "success")
        {
        }

        /// <summary>
        /// 服务器状态码
        /// </summary>
        [JsonProperty(PropertyName = "ret")]
        public RetEum Ret { get; set; }

        /// <summary>
        /// 接口内部状态码
        /// </summary>
        [JsonProperty(PropertyName = "errcode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 简单信息描述
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
