using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class LogisticsRepository : RepositoryBase<LogisticsInfo>
    {
        const string TableName = "logistics";

        public LogisticsRepository() : base(TableName)
        {
        }

    }
}
