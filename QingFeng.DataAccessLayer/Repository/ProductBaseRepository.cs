using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;
using QingFeng.Common.Extensions;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductBaseRepository : RepositoryBase<ProductBase>
    {
        const string TableName = "productBase";

        public ProductBaseRepository() : base(TableName)
        {
        }

        public IEnumerable<ProductBase> GetListByIds(params int[] baseId)
        {
            if (baseId == null || !baseId.Any())
            {
                return new List<ProductBase>();
            }

            var additional = $"AND baseId IN ({string.Join(",", baseId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<ProductBase>(null, TableName, buildWhereSql);
            }
        }

        public IEnumerable<ProductBase> SearchProductBase(int categoryId, string keyWords, int page, int pageSize,
            out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(keyWords)
                ? string.Empty
                : "AND (BaseName LIKE @keyWords OR BaseNo LIKE @keyWords)";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            object condition = null;

            if (categoryId > 0 && !string.IsNullOrWhiteSpace(keyWords))
            {
                condition = new {categoryId, keyWords = keyWords.FormatSqlLikeString()};
            }
            else if (categoryId > 0)
            {
                condition = new {categoryId};
            }
            else if (!string.IsNullOrWhiteSpace(keyWords))
            {
                condition = new {keyWords = keyWords.FormatSqlLikeString()};
            }

            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<ProductBase>(condition, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }
    }
}

