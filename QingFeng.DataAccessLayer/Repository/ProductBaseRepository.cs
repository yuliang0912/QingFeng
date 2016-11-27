using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;
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

        public IEnumerable<ProductBase> SearchProductBase(dynamic condition, int page, int pageSize,
            out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(condition.keyWords)
                ? string.Empty
                : "AND (BaseName LIKE @keyWords OR BaseNo LIKE @keyWords)";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            var conditionObj = condition as object;
            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<ProductBase>(conditionObj, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }
    }
}

