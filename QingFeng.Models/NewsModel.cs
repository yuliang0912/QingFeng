using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class NewsModel
    {
        [IgnoreField]
        public int NewsId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string CreateUserId { get; set; }

        public string CreateDate { get; set; }

        public int Status { get; set; }
    }
}
