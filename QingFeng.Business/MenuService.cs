using QingFeng.Common;
using System.Collections.Generic;
using static QingFeng.Common.AgentEnums;

namespace QingFeng.Business
{
    public class MenuService : Singleton<MenuService>
    {
        private MenuService()
        {
        }

        private Dictionary<MenuEnum, List<SubMenuEnum>> _allMenus;

        public Dictionary<MenuEnum, List<SubMenuEnum>> GetList()
        {
            return _allMenus ?? (_allMenus = new Dictionary<MenuEnum, List<SubMenuEnum>>
            {
                {
                    MenuEnum.商品中心, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.商品列表,
                        SubMenuEnum.添加商品,
                        SubMenuEnum.查看商品,
                        SubMenuEnum.编辑商品,
                        SubMenuEnum.上下架商品,
                        SubMenuEnum.代理商价格,
                        SubMenuEnum.代理商导入,
                        SubMenuEnum.设置代理商价格,
                    }
                },
                {
                    MenuEnum.库存中心, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.库存查询,
                        SubMenuEnum.导入库存,
                        SubMenuEnum.设置库存,
                    }
                },
                {
                    MenuEnum.订单中心, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.添加订单,
                        SubMenuEnum.订单列表,
                        SubMenuEnum.已取消订单,
                        SubMenuEnum.待支付订单,
                        SubMenuEnum.待发货订单,
                        SubMenuEnum.已完成订单,
                        SubMenuEnum.已发货订单,
                        SubMenuEnum.异常订单,
                        SubMenuEnum.订单导出,
                        SubMenuEnum.完成订单,
                        SubMenuEnum.发货,
                        SubMenuEnum.取消订单,
                        SubMenuEnum.异常取消,
                        SubMenuEnum.无货标记,
                        SubMenuEnum.修改地址,
                        SubMenuEnum.查看订单,
                        SubMenuEnum.添加备注,
                        SubMenuEnum.确认收款
                    }
                },
                {
                    MenuEnum.店铺管理, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.删除店铺,
                        SubMenuEnum.编辑店铺,
                        SubMenuEnum.添加店铺,
                        SubMenuEnum.店铺列表
                    }
                },
                {
                    MenuEnum.人员管理, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.员工列表,
                        SubMenuEnum.代理商列表,
                        SubMenuEnum.删除代理商,
                        SubMenuEnum.重置代理商密码,
                        SubMenuEnum.编辑代理商,
                        SubMenuEnum.添加代理商,
                        SubMenuEnum.删除员工,
                        SubMenuEnum.编辑员工,
                        SubMenuEnum.添加员工,
                        SubMenuEnum.设置权限,
                    }
                },
                {
                    MenuEnum.财务中心, new List<SubMenuEnum>()
                    {
                        SubMenuEnum.支付订单,
                        SubMenuEnum.导出支付流水,
                        SubMenuEnum.支付详情,
                    }
                }
            });
        }
    }
}
