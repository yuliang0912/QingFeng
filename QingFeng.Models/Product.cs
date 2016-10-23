using System;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    /// <summary>
    /// 产品基础信息
    /// </summary>
    public class Product
    {
        [IgnoreField]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int BaseId { get; set; }

        public string BaseName { get; set; }

        public string ProductNo { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }

        public string ImgList { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
