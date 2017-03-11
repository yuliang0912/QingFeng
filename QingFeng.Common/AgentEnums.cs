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
            /// 员工(普通管理员)
            /// </summary>
            Staff = 2,
            /// <summary>
            /// 分销商
            /// </summary>
            StoreUser = 3,
            /// <summary>
            /// 所有人
            /// </summary>
            AllUser = 4,
        }

        /// <summary>
        /// 分类
        /// </summary>
        public enum Category
        {
            休闲鞋 = 1,
            正装鞋 = 2,
            凉鞋 = 3,
            户外鞋 = 4,
            帆布鞋 = 5,
            高尔夫鞋 = 6,
            拖鞋 = 7,
            长靴 = 8,
            中短靴 = 9,
            高跟鞋 = 10,
            平底鞋 = 11,
            运动鞋 = 12,
            童鞋 = 13,
            帆船鞋 = 14,
            登山鞋 = 14,
            户外运动鞋 = 16
        }

        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            男 = 1,
            女 = 2,
            通用 = 3
        }

        /// <summary>
        /// 品牌
        /// </summary>
        public enum Brand
        {
            Locaste = 1
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public enum Warehouse
        {
            E_TOP_XH = 1
        }

        /// <summary>
        /// 主订单状态
        /// </summary>
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
            /// 待发货
            /// </summary>
            待发货 = 4,
            /// <summary>
            /// 已完成
            /// </summary>
            已完成 = 5,
            全部 = 6
        }

        /// <summary>
        /// 子订单状态
        /// </summary>
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
