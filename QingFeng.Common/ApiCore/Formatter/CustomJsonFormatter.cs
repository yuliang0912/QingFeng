using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using QingFeng.Common.ApiCore.Excepition;
using QingFeng.Common.ApiCore.Result;

namespace QingFeng.Common.ApiCore.Formatter
{
    /// <summary>
    /// 自定义
    /// </summary>
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger("StoreSaas.OpenAPI");
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly Encoding _encode = null;

        public CustomJsonFormatter(JsonSerializerSettings jsonSerializerSettings = null)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? JsonHelper.Settings;
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            _encode = new UTF8Encoding(false, true);
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream,
            System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);
            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(readStream, _encode))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream,
            System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);
            return Task.Factory.StartNew(() =>
            {
                using (var streamWriter = new StreamWriter(writeStream, _encode))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        ApiResult result;
                        if (value != null && value is ApiException)
                        {
                            var apiException = (ApiException) value;
                            result = new ApiResult()
                            {
                                Ret = apiException.Ret,
                                ErrorCode = apiException.ErrCode,
                                Message = apiException.Message
                            };
                        }
                        else if (value != null && value is ApiResult)
                        {
                            result = (ApiResult) value;
                        }
                        else if (value != null && value is HttpError)
                        {
                            var exception = (HttpError) value;

                            var message = string.Join(",", exception.Select(t => string.Concat(t.Key, ":", t.Value)));
                            result = new ApiResult() {Ret = RetEum.HttpError, ErrorCode = 1, Message = message};
                            _logger.Error("系统异常:" + result.Message);
                        }
                        else
                        {
                            result = new ApiResult<object>() {Data = value, Message = "success"};
                        }
                        serializer.Serialize(jsonTextWriter, result);
                    }
                }
            });
        }
    }
}