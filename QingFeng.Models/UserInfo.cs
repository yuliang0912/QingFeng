﻿using System;
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

        public AgentEnums.UserRole UserRole { get; set; }

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
                var list = new List<SubMenuEnum>();
                foreach (var menu in UserMenus.Split(',').Select(int.Parse))
                {
                    if (Enum.IsDefined(typeof(SubMenuEnum), menu))
                    {
                        list.Add((SubMenuEnum)menu);
                    }
                }
                return list;
            }
        }
    }
}
