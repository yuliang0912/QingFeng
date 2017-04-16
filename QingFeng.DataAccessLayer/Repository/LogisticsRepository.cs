using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common.Dapper;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class LogisticsRepository : RepositoryBase<LogisticsInfo>
    {
        const string TableName = "logistics";

        public LogisticsRepository() : base(TableName)
        {
        }

        public IEnumerable<LogisticsInfo> GetBatchLogistics(params long[] orderId)
        {
            if (orderId == null || !orderId.Any())
            {
                return new List<LogisticsInfo>();
            }

            var additional = $"AND orderId IN ({string.Join(",", orderId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<LogisticsInfo>(null, TableName, buildWhereSql);
            }
        }
    }
}
