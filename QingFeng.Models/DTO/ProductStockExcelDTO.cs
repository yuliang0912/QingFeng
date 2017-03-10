using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Models.DTO
{
    public class ProductStockExcelDTO
    {
        [JsonProperty(PropertyName = "商品ID")]
        public int BaseId { get; set; }

        [JsonProperty(PropertyName = "spu_id")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "sku_id")]
        public int SkuId { get; set; }

        [JsonProperty(PropertyName = "stock_id")]
        public int StockId { get; set; }

        [JsonProperty(PropertyName = "货号")]
        public string BaseNo { get; set; }

        [JsonProperty(PropertyName = "颜色")]
        public string ProductNo { get; set; }

        [JsonProperty(PropertyName = "尺码")]
        public string SkuName { get; set; }

        [JsonProperty(PropertyName = "库存")]
        public int StockNum { get; set; }
    }
}
