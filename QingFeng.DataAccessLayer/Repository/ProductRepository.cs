using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;
using QingFeng.Common.Extensions;
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

        public IEnumerable<Product> SearchProduct(string keyWords, int page, int pageSize,
            out int totalItem)
        {
            totalItem = 0;
            if (string.IsNullOrWhiteSpace(keyWords))
            {
                return new List<Product>();
            }

            var additional = "AND productNo LIKE @keyWords AND status = 0";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<Product>(new {keyWords = keyWords.FormatSqlLikeString()}, TableName,
                    "CreateDate DESC",
                    page, pageSize, out totalItem, buildWhereSql);
            }
        }
    }
}
