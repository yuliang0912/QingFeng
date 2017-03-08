using QingFeng.Common.Dapper;
using System;

namespace QingFeng.Models
{
    /// <summary>
    /// 自定义价格
    /// </summary>
    public class UserProductPrice
    {
        [IgnoreField]
        public int PriceId { get; set; }

        public int BaseId { get; set; }

        public int BrandId { get; set; }

        public int ProductId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal ActualPrice { get; set; }

        public int UserId { get; set; }

        public int Status { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
