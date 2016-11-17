using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common;

namespace QingFeng.Models
{
    public class OrderMaster
    {
        public OrderMaster()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public int OrderId { get; set; }

        //淘宝的订单编号
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 订单优惠
        /// </summary>		
        public decimal OrderFavorable { get; set; }

        /// <summary>
        /// 子商品个数
        /// </summary>
        public int OrderDetailCount { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 店铺所有人
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// 地址区域码
        /// </summary>
        public int AreaCode { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public AgentEnums.MasterOrderStatus OrderStatus { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 子订单详情
        /// </summary>
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
