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
            /// 待发货
            /// </summary>
            已发货 = 5,

            /// <summary>
            /// 已完成
            /// </summary>
            已完成 = 6,
            全部 = 7
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
            无货取消 = 4,

            /// <summary>
            /// 无货异常
            /// </summary>
            无货异常 = 5,
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

        public enum PayType
        {
            智付 = 1
        }

        public enum PayStatus
        {
            待支付 = 1,
            支付成功 = 2,
            支付失败 = 3
        }

        public enum VerifyStatus
        {
            未审核 = 1,
            已审核 = 2,
            审核失败 = 3
        }

        public enum MenuEnum
        {
            店铺管理 = 1,
            人员管理 = 2,
            商品中心 = 3,
            库存中心 = 4,
            订单中心 = 5,
            财务中心 = 6
        }

        public enum SubMenuEnum
        {
            全部 = 0,
            删除店铺 = 101,
            编辑店铺 = 102,
            添加店铺 = 103,
            店铺管理 = 104,
            删除代理商 = 201,
            重置代理商密码 = 202,
            编辑代理商 = 203,
            添加代理商 = 204,
            代理商列表 = 205,
            删除员工 = 206,
            编辑员工 = 207,
            添加员工 = 208,
            设置权限 = 209,
            员工列表 = 210,
            代理商导入 = 301,
            设置代理商价格 = 302,
            代理商价格 = 303,
            查看商品 = 304,
            商品列表 = 305,
            导入库存 = 401,
            编辑库存 = 402,
            添加库存 = 403,
            库存查询 = 404,
            订单导出 = 501,
            订单完成 = 502,
            发货 = 503,
            取消订单 = 504,
            异常取消 = 505,
            无货标记 = 506,
            修改地址 = 507,
            查看订单 = 508,
            添加备注 = 509,
            查看备注 = 510,
            已取消订单 = 511,
            待支付订单 = 512,
            待发货订单 = 513,
            已完成订单 = 514,
            已发货订单 = 515,
            异常订单 = 516,
            订单列表 = 517,
            添加订单 = 518,
            导出支付流水 = 601,
            支付详情 = 602,
            支付订单 = 603
        }
    }
}
