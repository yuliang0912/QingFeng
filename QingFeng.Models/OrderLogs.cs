using System;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class OrderLogs
    {
        [IgnoreField]
        public int LogId { get; set; }

        public long OrderId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
