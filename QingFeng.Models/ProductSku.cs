using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class ProductSku
    {
        [IgnoreField]
        public int Id { get; set; }

        public int SkuId { get; set; }

        public int BaseId { get; set; }

        public int ProductId { get; set; }
    }
}
