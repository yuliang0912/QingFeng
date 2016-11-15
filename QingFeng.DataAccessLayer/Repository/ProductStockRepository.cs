using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductStockRepository : RepositoryBase<ProductStock>
    {
        const string TableName = "productStock";

        public ProductStockRepository() : base(TableName)
        {
        }


        public IEnumerable<ProductStock> GetProductStockListByBaseIds(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<ProductStock>();
            }

            var additional = $"AND baseId IN ('{string.Join("','", baseId)}')";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<ProductStock>(null, TableName, buildWhereSql);
            }
        }

        public bool SetProductStock(int productId, List<KeyValuePair<int, int>> sizeSku)
        {
            var count = 0;
            using (var connection = GetReadConnection)
            {
                count += sizeSku.Sum(item => connection.Update(new {stockNum = item.Value}, new {productId, skuId = item.Key}, TableName));
            }
            return count > 0;
        }
    }
}
