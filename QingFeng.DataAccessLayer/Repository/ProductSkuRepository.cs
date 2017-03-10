using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Models;
using QingFeng.Common.Dapper;
using Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductSkuRepository : RepositoryBase<ProductSkus>
    {
        const string TableName = "productSkus";

        public ProductSkuRepository() : base(TableName)
        {

        }

        public IEnumerable<ProductSkus> GetProductSkuListByBaseIds(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<ProductSkus>();
            }

            var additional = $"AND baseId IN ({string.Join(",", baseId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<ProductSkus>(null, TableName, buildWhereSql);
            }
        }

        public List<KeyValuePair<int, string>> GetDistinctSkuList(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<KeyValuePair<int, string>>();
            }

            var sql = $"SELECT DISTINCT skuId,skuName FROM productskus WHERE baseId IN ({string.Join(",", baseId)}) ORDER BY skuId";

            using (var connection = GetReadConnection)
            {
                return connection.Query<ProductStock>(sql)
                    .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                    .ToList();
            }
        }
    }
}
