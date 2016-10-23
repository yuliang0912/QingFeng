using System;

namespace QingFeng.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int UserRole { get; set; }

        public string PassWord { get; set; }

        public string Salt { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
