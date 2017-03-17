using QingFeng.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QingFeng.Common.AgentEnums;

namespace QingFeng.Business
{
    public class MenuService : Singleton<MenuService>
    {
        public Dictionary<MenuEnum, List<SubMenuEnum>> GetList()
        {
            var dict = new Dictionary<MenuEnum, List<SubMenuEnum>>();
            dict.Add(MenuEnum.用户管理, new List<SubMenuEnum>()
            {
                SubMenuEnum.添加
            });
            return dict;
        }
    }
}
