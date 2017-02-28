using QingFeng.Common.Dapper;
using System;

namespace QingFeng.Models
{
    public class UserLogs
    {
        [IgnoreField]
        public int LogId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
