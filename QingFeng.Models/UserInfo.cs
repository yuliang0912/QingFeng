﻿using System;
using System.Collections.Generic;
using QingFeng.Common;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            this.StoreList=new List<StoreInfo>();
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

        [IgnoreField]
        public List<StoreInfo> StoreList { get; set; }
    }
}
