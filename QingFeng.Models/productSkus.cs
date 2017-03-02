using System;

namespace QingFeng.Models
{
    public class ProductSkus
    {
        public int Id { get; set; }

        public int SkuId { get; set; }

        public int BaseId { get; set; }

        public int ProductId { get; set; }

        public int Status { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
