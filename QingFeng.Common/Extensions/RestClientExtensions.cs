using System;
using RestSharp;

namespace QingFeng.Common.Extensions
{

    #region RestClient扩展方法

    /// <summary>
    /// RestClient扩展方法
    /// </summary>
    public class RestClientExtension
    {
        private readonly string _baseUrl;

        public RestClientExtension(string apiServer)
        {
            _baseUrl = apiServer;
        }

        public T ExecuteGet<T>(string resource, object body = null) where T : new()
        {
            return Execute<T>(resource, Method.GET, body);
        }

        public T ExecutePost<T>(string resource, object body = null) where T : new()
        {
            return Execute<T>(resource, Method.POST, body);
        }

        public string ExecuteBodyPost(string resource, object body = null)
        {
            var client = new RestClient(_baseUrl);
            client.CookieContainer = new System.Net.CookieContainer();
            var request = BuildRequest(resource, Method.POST, body, true);
            var response = client.Execute(request);
            return response.Content;
        }

        public RestRequestAsyncHandle ExecuteBodyPostAsync(string resource, object body = null,
            Action<IRestResponse, RestRequestAsyncHandle> callback = null)
        {
            var client = new RestClient(_baseUrl);
            client.CookieContainer = new System.Net.CookieContainer();
            var request = BuildRequest(resource, Method.POST, body, true);
            return client.ExecuteAsync(request, callback);
        }

        public T Execute<T>(string resource, Method method = Method.GET, object body = null)
            where T : new()
        {
            var client = new RestClient(_baseUrl) {CookieContainer = new System.Net.CookieContainer()};
            client.AddHandler("application/json", new CustomJsonDeserializer());
            var request = BuildRequest(resource, method, body);
            return client.Execute<T>(request).Data;
        }

        /// <summary>
        /// 执行API返回字符串
        /// </summary>
        /// <param name="resource">API地址</param>
        /// <param name="method">请求方式,默认GET</param>
        /// <param name="body">参数实体</param>
        /// <returns>结果字符串</returns>
        public string ExecuteContent(string resource, Method method = Method.GET, object body = null)
        {
            var client = new RestClient(_baseUrl);
            client.CookieContainer = new System.Net.CookieContainer();
            var request = BuildRequest(resource, method, body);
            var response = client.Execute(request);
            return response.Content;
        }

        private RestRequest BuildRequest(string resource, Method method = Method.GET, object body = null,
            bool isBodyPost = false)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException("请求资源url不能为空。");
            }

            var request = new RestRequest(method)
            {
                Resource = resource.ToLower(),
                RequestFormat = DataFormat.Json,
                DateFormat = "yyyy-MM-ddTHH:mm:ss",
                JsonSerializer = new CustomJsonSerializer()
            };

            //01.Header参数
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            //02.用户参数及.业务参数
            if (body != null)
            {
                if (isBodyPost == false)
                {
                    request.AddObject(body);
                }
                else
                {
                    request.AddBody(body);
                }
            }

            return request;
        }
    }

    #endregion

    #region 自定义反序列化结果类

    public class CustomJsonDeserializer : RestSharp.Deserializers.IDeserializer
    {
        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonHelper.Decode<T>(response.Content);
        }
    }

    #endregion

    #region 自定义序列化结果类

    public class CustomJsonSerializer : RestSharp.Serializers.ISerializer
    {
        public CustomJsonSerializer()
        {
            ContentType = "application/json";
        }

        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            return JsonHelper.Encode<dynamic>(obj);
        }
    }

    #endregion

}
