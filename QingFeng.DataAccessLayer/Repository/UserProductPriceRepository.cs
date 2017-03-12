using QingFeng.Common.Dapper;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.DataAccessLayer.Repository
{
    public class UserProductPriceRepository : RepositoryBase<UserProductPrice>
    {
        const string TableName = "userProductPrice";

        public UserProductPriceRepository() : base(TableName)
        {
        }


        public IEnumerable<UserProductPrice> GetListByBaseIds(int userId, params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<UserProductPrice>();
            }

            var additional = $"AND baseId IN ({string.Join(",", baseId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<UserProductPrice>(new {userId}, TableName, buildWhereSql);
            }
        }

        public bool BatchInsert(int userId, int baseId, List<UserProductPrice> list)
        {
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    connection.Delete(new {userId, baseId}, TableName, transaction: trans);
                    foreach (var item in list)
                    {
                        connection.Insert(item, TableName, trans);
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return true;
        }
    }
}
