using System.Collections.Generic;
using QingFeng.Common;

namespace QingFeng.Models.DTO
{
    public class CreateProductDto
    {
        public int baseId { get; set; }

        public string baseNo { get; set; }

        public string baseName { get; set; }

        public AgentEnums.Brand brandId { get; set; }

        public AgentEnums.Category categoryId { get; set; }

        public AgentEnums.Sex sex { get; set; }

        public List<SubProduct> subProduct { get; set; }
    }

    public class SubProduct
    {
        public string color { get; set; }

        public decimal lowestPrice { get; set; }

        public List<SizeInfo> sizeList { get; set; }
    }

    public class SizeInfo
    {
        public int sizeId { get; set; }

        public string sizeName { get; set; }

        public decimal sizePrice { get; set; }
    }
}
