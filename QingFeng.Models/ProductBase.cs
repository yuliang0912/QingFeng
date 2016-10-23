using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common.Dapper;

namespace QingFeng.Models
{
    public class ProductBase
    {
        [IgnoreField]
        public int BaseId { get; set; }

        public string BaseName { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal ActualPrice { get; set; }

        public string BaseNo { get; set; }

        public string ImgList { get; set; }

        public string Intro { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
