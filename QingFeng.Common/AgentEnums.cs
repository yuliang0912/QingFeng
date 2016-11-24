using System;
using System.Runtime.Serialization;

namespace QingFeng.Common
{
    public class AgentEnums
    {
        public enum SkuType
        {
            //颜色
            Color = 1,
            //尺码
            Size = 2
        }

        public enum UserRole
        {
            Administrator = 1,
            StoreUser = 2,
            AllUser = 3,
        }

        public enum Category
        {
            /// <summary>
            /// 男士
            /// </summary>
            男鞋 = 1,

            /// <summary>
            /// 女士
            /// </summary>
            女鞋 = 2
        }

        public enum MasterOrderStatus
        {
            /// <summary>
            /// 已取消
            /// </summary>
            Canceled = 1,

            /// <summary>
            /// 异常
            /// </summary>
            Exception = 2,

            /// <summary>
            /// 待支付
            /// </summary>
            WaitPay = 3,

            /// <summary>
            /// 已支付
            /// </summary>
            AlreadyPay = 4,

            /// <summary>
            /// 待发货
            /// </summary>
            WaitDeliverGoods = 5,

            /// <summary>
            /// 进行中
            /// </summary>
            Doing = 6,

            /// <summary>
            /// 已完成
            /// </summary>
            Completed = 7
        }

        public enum OrderDetailStatus
        {
            /// <summary>
            /// 待发货
            /// </summary>
            WaitDeliverGoods = 1,

            /// <summary>
            /// 已发货
            /// </summary>
            HasDeliverGoods = 2,

            /// <summary>
            /// 已取消
            /// </summary>
            Canceled = 3,
        }
    }
}
