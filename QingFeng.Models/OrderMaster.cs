using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class OrderMaster
    {
        public OrderMaster()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public long OrderId { get; set; }

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
        /// 是否自营
        /// </summary>
        public int IsSelfSupport { get; set; }

        public string StoreName { get; set; }

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
        /// 省级ID
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaCode { get; set; }

        /// <summary>
        /// 地址前缀(XX省 XX市 XX区)
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 收货地址(街道门牌号)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public AgentEnums.MasterOrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 支付流水号
        /// </summary>
        public string PayNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 子订单详情
        /// </summary>
        [IgnoreField]
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
