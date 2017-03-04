using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Models;
using QingFeng.Common.Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductSkuRepository : RepositoryBase<ProductSkus>
    {
        const string TableName = "productSkus";

        public ProductSkuRepository() : base(TableName)
        {

        }

        public IEnumerable<ProductStock> GetProductStockListByBaseIds(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<ProductStock>();
            }

            var additional = $"AND baseId IN ({string.Join(",", baseId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<ProductStock>(null, TableName, buildWhereSql);
            }
        }
    }
}
