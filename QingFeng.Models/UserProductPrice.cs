using QingFeng.Common.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
