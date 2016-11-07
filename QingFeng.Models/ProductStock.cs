using QingFeng.Common.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Models
{
    /// <summary>
    /// 产品库存
    /// </summary>
    public class ProductStock
    {
        [IgnoreField]
        public int StockId { get; set; }

        public int BaseId { get; set; }

        public int ProductId { get; set; }

        public int SkuId { get; set; }

        public string SkuName { get; set; }

        public int StockNum { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
