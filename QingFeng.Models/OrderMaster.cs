using System;
using System.Collections.Generic;
using System.Linq;

namespace QingFeng.Models
{
    public class OrderMaster
    {
        public OrderMaster()
        {
            this.OrderDetails = Enumerable.Empty<OrderDetail>();
        }

        public int OrderId { get; set; }

        //taobao
        public string OrderNo { get; set; }

        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 订单优惠
        /// </summary>		
        public decimal OrderFavorable { get; set; }

        public int StoreId { get; set; }

        public int UserId { get; set; }

        public int OrderStatus { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>	
        public int PayType { get; set; }

        /// <summary>
        /// 支付手续费
        /// </summary>	
        public decimal PayFee { get; set; }

        /// <summary>
        /// 支付状态,1未付款，2已付款
        /// </summary>		
        public int PayStatus { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>		
        public DateTime PayDate { get; set; }

        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
