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
            /// <summary>
            /// 超级管理员
            /// </summary>
            Administrator = 1,
            /// <summary>
            /// 员工
            /// </summary>
            Staff = 2,
            /// <summary>
            /// 分销商
            /// </summary>
            StoreUser = 2,
            /// <summary>
            /// 自营
            /// </summary>
            SelfSupport = 3,
            /// <summary>
            /// 所有人
            /// </summary>
            AllUser = 4,
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
            已取消 = 1,

            /// <summary>
            /// 异常
            /// </summary>
            异常 = 2,

            /// <summary>
            /// 待支付
            /// </summary>
            待支付 = 3,

            /// <summary>
            /// 已支付
            /// </summary>
            已支付 = 4,

            /// <summary>
            /// 待发货
            /// </summary>
            待发货 = 5,

            /// <summary>
            /// 进行中
            /// </summary>
            进行中 = 6,

            /// <summary>
            /// 已完成
            /// </summary>
            已完成 = 7
        }

        public enum OrderDetailStatus
        {
            /// <summary>
            /// 待发货
            /// </summary>
            待发货 = 1,

            /// <summary>
            /// 已发货
            /// </summary>
            已发货 = 2,

            /// <summary>
            /// 已取消
            /// </summary>
            已取消 = 3,
            /// <summary>
            /// 无货标记
            /// </summary>
            无货标记 = 4,
        }

        public enum ShopTypeEnum
        {
            淘宝 = 1,
            天猫 = 2,
            天猫国际 = 3,
            京东 = 4,
            京东国际 = 5,
            当当 = 6,
            唯品会 = 7,
            聚美优品 = 8,
            一号店 = 9,
            工商银行 = 10,
            苏宁 = 11,
        }
    }
}
