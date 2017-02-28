using System;
using QingFeng.Common.Dapper;
using QingFeng.Common;

namespace QingFeng.Models
{
    public class StoreInfo
    {
        [IgnoreField]
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        public AgentEnums.ShopTypeEnum ShopType { get; set; }

        public int MasterUserId { get; set; }

        public string MasterUserName { get; set; }

        public string HomeUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
