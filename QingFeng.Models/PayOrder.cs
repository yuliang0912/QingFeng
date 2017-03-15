using System;
using QingFeng.Common;

namespace QingFeng.Models
{
    public class PayOrder
    {
        public string PayNo { get; set; }


        public long OrderId { get; set; }


        public string OrderNo { get; set; }

        /// <summary>
        /// 支付平台ID
        /// </summary>
        public string OutsideId { get; set; }


        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal CounterFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AgentEnums.PayType PayType { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public AgentEnums.PayStatus PayStatus { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AgentEnums.VerifyStatus VerifyStatus { get; set; }


        public int UserId { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime PayDate { get; set; }

        public DateTime VerifyDate { get; set; }
    }
}
