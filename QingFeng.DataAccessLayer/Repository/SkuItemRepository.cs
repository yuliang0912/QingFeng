using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common.Dapper;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class SkuItemRepository : RepositoryBase<SkuItem>
    {
        private const string TableName = "skuItems";

        public SkuItemRepository() : base(TableName)
        {
        }

        public IEnumerable<SkuItem> GetListByIds(params int[] skuId)
        {
            if (skuId == null || !skuId.Any())
            {
                return new List<SkuItem>();
            }

            var additional = $"AND skuId IN ({string.Join(",", skuId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<SkuItem>(null, TableName, buildWhereSql);
            }
        }

        public bool BatchInsert(List<SkuItem> list)
        {
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
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
