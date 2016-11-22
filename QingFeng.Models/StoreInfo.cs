using System;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class StoreInfo
    {
        [IgnoreField]
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        public int MasterUserId { get; set; }

        public string HomeUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
