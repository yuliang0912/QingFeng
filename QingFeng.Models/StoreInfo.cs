using System;

namespace QingFeng.Models
{
    public class StoreInfo
    {
        public int StroeId { get; set; }

        public string StoreName { get; set; }

        public int UserId { get; set; }

        public string HomeUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
