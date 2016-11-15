using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace QingFeng.Common.ApiCore.Result
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPageList<T>
    {
        public ApiPageList()
        {
            PageList = Enumerable.Empty<T>();
        }

        /// <summary>
        /// 页码(从1开始)
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 总数据量
        /// </summary>
        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount
        {
            get
            {
                if (TotalCount < 1 || PageSize < 1)
                {
                    return 0;
                }
                return (int)Math.Ceiling(TotalCount * 1.0 / PageSize);
            }
        }

        [JsonProperty(PropertyName = "pageList")]
        public IEnumerable<T> PageList { get; set; }
    }
}