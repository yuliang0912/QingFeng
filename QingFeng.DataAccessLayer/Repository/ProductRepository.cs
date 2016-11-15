using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductRepository : RepositoryBase<Product>
    {
        const string TableName = "product";

        public ProductRepository() : base(TableName)
        {
        }

        public IEnumerable<Product> GetProductListByBaseIds(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<Product>();
            }

            var additional = $"AND baseId IN ('{string.Join("','", baseId)}') AND status = 0";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<Product>(null, TableName, buildWhereSql);
            }
        }
    }
}
