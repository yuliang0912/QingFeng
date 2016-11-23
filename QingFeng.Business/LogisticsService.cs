using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Business
{
    public class LogisticsService
    {
        public List<KeyValuePair<int, string>> GetComplanyList()
        {
            return new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "顺丰速运"),
                new KeyValuePair<int, string>(2, "邮政EMS"),
                new KeyValuePair<int, string>(3, "圆通快递"),
                new KeyValuePair<int, string>(4, "申通快递"),
                new KeyValuePair<int, string>(5, "韵达快递"),
                new KeyValuePair<int, string>(6, "中通快递"),
                new KeyValuePair<int, string>(7, "宅急送"),
                new KeyValuePair<int, string>(8, "天天快递"),
            };
        }
    }
}
