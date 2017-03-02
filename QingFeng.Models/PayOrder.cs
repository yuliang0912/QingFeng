using System;

namespace QingFeng.Models
{
    public class PayOrder
    {
        public string PayNo { get; set; }

        public string OrderId { get; set; }

        /// <summary>
        /// 支付平台ID
        /// </summary>
        public string OutsideId { get; set; }

        public decimal ActualPrice { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal CounterFee { get; set; }

        public int PayType { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int VerifyStatus { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
