using System.Collections.Generic;
using QingFeng.Common.Dapper;
using static QingFeng.Common.AgentEnums;
using QingFeng.Common;

namespace QingFeng.Models
{
    public class OrderDetail
    {
        /// <summary>
        /// 流水号
        /// </summary>		
        public int FlowId { get; set; }

        public long OrderId { get; set; }

        public string OrderNo { get; set; }

        public int SkuId { get; set; }

        public string SkuName { get; set; }

        public AgentEnums.Brand BrandId { get; set; }

        public int BaseId { get; set; }

        public string BaseNo { get; set; }

        public string BaseName { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>		
        public int ProductId { get; set; }

        //商品编号
        public string ProductNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>		
        public string ProductName { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>		
        public decimal Price { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>		
        public int Quantity { get; set; }

        /// <summary>
        /// 商品金额
        /// </summary>		
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>		
        public OrderDetailStatus OrderStatus { get; set; }

        [IgnoreField]
        public LogisticsInfo LogisticsInfo { get; set; }
    }
}
