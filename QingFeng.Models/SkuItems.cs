using QingFeng.Common;

namespace QingFeng.Models
{
    public class SkuItems
    {
        public int SkuId { get; set; }

        public string SkuName { get; set; }

        public string SkuImgUrl { get; set; }

        public AgentEnums.SkuType SkuType { get; set; }

        public int OrderId { get; set; }

        public int Status { get; set; }
    }
}
