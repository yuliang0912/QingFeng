using System;

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
        public Common.AgentEnums.PayType PayType { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public Common.AgentEnums.PayStatus Status { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int VerifyStatus { get; set; }


        public int UserId { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime PayDate { get; set; }

        public DateTime VerifyDate { get; set; }
    }
}
