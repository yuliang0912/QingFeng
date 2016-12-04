using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common.Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class StoreRepository : RepositoryBase<StoreInfo>
    {

        const string TableName = "storeInfo";
        public StoreRepository() : base("storeInfo") { }

        public IEnumerable<StoreInfo> GetBatchStoreInfos(params int[] storeId)
        {
            if (storeId == null || !storeId.Any())
            {
                return new List<StoreInfo>();
            }

            var additional = $"AND masterUserId IN ({string.Join(",", storeId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<StoreInfo>(null, TableName, buildWhereSql);
            }
        }
    }
}
