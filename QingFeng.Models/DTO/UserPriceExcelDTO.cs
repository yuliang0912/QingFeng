using Newtonsoft.Json;

namespace QingFeng.Models.DTO
{
    public class UserPriceExcelDto
    {
        [JsonProperty(PropertyName = "商品ID")]
        public int BaseId { get; set; }

        [JsonProperty(PropertyName = "spu_id")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "货号")]
        public string BaseNo { get; set; }

        [JsonProperty(PropertyName = "颜色")]
        public string ProductNo { get; set; }

        [JsonProperty(PropertyName = "市场价")]
        public decimal OriginalPrice { get; set; }

        [JsonProperty(PropertyName = "供货价")]
        public decimal ActualPrice { get; set; }
    }
}
