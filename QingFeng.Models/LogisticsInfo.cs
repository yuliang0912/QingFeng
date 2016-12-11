using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class LogisticsInfo
    {
        [IgnoreField]
        public int Id { get; set; }

        //订单号
        public long OrderId { get; set; }

        public string FlowIds { get; set; }

        //物流公司ID
        public int CompanyId { get; set; }

        //物流公司
        public string CompanyName { get; set; }

        //物流单号
        public string OddNumber { get; set; }

        //运费
        public decimal Price { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [IgnoreField]
        public List<int> FlowIdList
            => string.IsNullOrWhiteSpace(FlowIds) ? new List<int>() : FlowIds.Split(',').Select(int.Parse).ToList();
    }
}
