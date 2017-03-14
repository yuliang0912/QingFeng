using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Models.DTO
{
    public class PayOrderExcelDTO
    {
        [JsonProperty(PropertyName = "支付流水号")]
        public int PayNo { get; set; }

        [JsonProperty(PropertyName = "支付类型")]
        public string PayType { get; set; }

        [JsonProperty(PropertyName = "订单金额")]
        public decimal PayMoment { get; set; }

        [JsonProperty(PropertyName = "订单流水号")]
        public long OrderId { get; set; }

        [JsonProperty(PropertyName = "订单号")]
        public string OrderNo { get; set; }

        [JsonProperty(PropertyName = "平台流水号")]
        public string OutsideId { get; set; }

        [JsonProperty(PropertyName = "支付时间")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "审核时间")]
        public DateTime VerifyDate { get; set; }

        [JsonProperty(PropertyName = "审核状态")]
        public string VerifyState { get; set; }

        [JsonProperty(PropertyName = "品牌")]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "货号")]
        public string BaseNo { get; set; }

        [JsonProperty(PropertyName = "颜色")]
        public string ProductNo { get; set; }
    }
}
