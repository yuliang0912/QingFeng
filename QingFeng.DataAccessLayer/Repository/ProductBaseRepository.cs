using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Dapper;
using QingFeng.Common.Extensions;
using QingFeng.Models;
using Dapper;

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

        public IEnumerable<ProductBase> SearchProductBase(int categoryId, string keyWords, int status, int page, int pageSize,
            out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(keyWords)
                ? string.Empty
                : "AND (BaseName LIKE @keyWords OR BaseNo LIKE @keyWords)";

            if (status > -1)
            {
                additional += " AND status = " + status;
            }

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            object condition = null;

            if (categoryId > 0 && !string.IsNullOrWhiteSpace(keyWords))
            {
                condition = new { categoryId, keyWords = keyWords.FormatSqlLikeString() };
            }
            else if (categoryId > 0)
            {
                condition = new { categoryId };
            }
            else if (!string.IsNullOrWhiteSpace(keyWords))
            {
                condition = new { keyWords = keyWords.FormatSqlLikeString() };
            }

            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<ProductBase>(condition, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productBase"></param>
        /// <param name="productSkus"></param>
        /// <returns></returns>
        public bool CreateProduct(ProductBase productBase, List<ProductSkus> productSkus)
        {
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    var baseId = connection.Insert(productBase, TableName, trans, isReturnIncrementId: true);
                    foreach (var item in productBase.SubProduct)
                    {
                        item.BaseId = baseId;
                        var productId = connection.Insert(item, "product", trans, isReturnIncrementId: true);
                        foreach (var sku in item.ProductSkus)
                        {
                            sku.BaseId = baseId;
                            sku.ProductId = productId;
                            connection.Insert(sku, "productskus", trans);
                            //var sql = @"INSERT INTO productskus(skuId,baseId,productId,price,status) 
                            //            VALUES({0},{1},{2},{3},0) ON DUPLICATE KEY UPDATE status = 0";
                            //sql = string.Format(sql, sku.SkuId, baseId, productId, sku.Price);
                            //connection.Execute(sql, null, trans);
                        }
                    }
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

