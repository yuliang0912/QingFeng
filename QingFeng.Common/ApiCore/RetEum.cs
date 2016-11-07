
namespace StoreSaas.Common.ApiCore
{
    public enum RetEum
    {
        /// <summary>
        /// 未定义错误
        /// </summary>
        UndefinedError = -2,
        /// <summary>
        /// HTTP请求错误
        /// </summary>
        HttpError = -1,
        /// <summary>
        /// 正常返回
        /// </summary>
        Success = 0,
        /// <summary>
        /// 鉴权失败
        /// </summary>
        AuthenticationFailure = 1,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        ServerError = 2,
        /// <summary>
        /// 应用程序错误
        /// </summary>
        ApplicationError = 3,
        /// <summary>
        /// 频率受限
        /// </summary>
        FrequencyLimit = 4,
    }

    public enum ClientIdEum
    {
        /// <summary>
        /// 苹果端
        /// </summary>
        IOS = 1001,
        /// <summary>
        /// 安卓端
        /// </summary>
        Android = 1002,
        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 1003
    }
}
