using System;
using System.Collections.Generic;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    /// <summary>
    /// 产品基础信息
    /// </summary>
    public class Product
    {

        public Product()
        {
            ProductStocks = new List<ProductStock>();
        }

        [IgnoreField]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int BaseId { get; set; }

        public string BaseNo { get; set; }

        public string BaseName { get; set; }

        public string ProductNo { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal ActualPrice { get; set; }

        public int ColorId { get; set; }

        /// <summary>
        /// 库存(所有尺码的)
        /// </summary>
        public int StockNum { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }

        public string ImgList { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }

        [IgnoreField]
        public IEnumerable<ProductStock> ProductStocks { get; set; }
    }
}
