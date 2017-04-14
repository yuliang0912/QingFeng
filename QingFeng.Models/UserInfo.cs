using System;
using System.Collections.Generic;
using QingFeng.Common;
using QingFeng.Common.Dapper;
using static QingFeng.Common.AgentEnums;
using System.Linq;

namespace QingFeng.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            this.StoreList = new List<StoreInfo>();
        }

        [IgnoreField]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }

        public UserRole UserRole { get; set; }

        public string Avatar { get; set; }

        public string PassWord { get; set; }

        public string Salt { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }

        public string UserMenus { get; set; }

        [IgnoreField]
        public List<StoreInfo> StoreList { get; set; }


        [IgnoreField]
        public List<SubMenuEnum> AllUserMenus
        {
            get
            {
                if (string.IsNullOrEmpty(UserMenus))
                {
                    return new List<SubMenuEnum>();
                }
                return (from menu in UserMenus.Split(',').Select(int.Parse)
                        where Enum.IsDefined(typeof(SubMenuEnum), menu)
                        select (SubMenuEnum)menu).ToList();
            }
        }

        [IgnoreField]
        public List<MenuEnum> UserFirstMenus
        {
            get
            {
                return this.AllUserMenus.Select(t => t.GetHashCode() / 100).OrderBy(t => t).
                    GroupBy(c => c).ToDictionary(c => c.Key, c => c)
                    .Select(t => (MenuEnum)t.Key).ToList();
            }
        }

        [IgnoreField]
        public Dictionary<SubMenuEnum, string> UserSubMenus
        {
            get
            {
                return this.AllUserMenus.ToDictionary(c => c, c => c.ToString());
            }
        }
    }
}
