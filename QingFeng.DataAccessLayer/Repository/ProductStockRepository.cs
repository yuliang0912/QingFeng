using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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
                count +=
                    sizeSku.Sum(
                        item =>
                            connection.Update(new { stockNum = item.Value }, new { productId, skuId = item.Key },
                                TableName));
            }
            return count > 0;
        }

        /// <summary>
        /// 原库存基础上做新增或者减少
        /// </summary>
        /// <param name="orderSkuNumbers"></param>
        /// <returns></returns>
        public bool UpdateProductStock(List<Tuple<int, int, int>> orderSkuNumbers)
        {
            using (var connection = GetReadConnection)
            {
                foreach (var item in orderSkuNumbers)
                {
                    var sql =
                        "Update productstock SET stockNum = stockNum + @addValue WHERE productId = @productId AND skuId = @skuId";
                    connection.Execute(sql, new {productId = item.Item1, skuId = item.Item2, addValue = item.Item3});
                }
            }
            return true;
        }

        public bool BatchInsert(List<ProductStock> list)
        {
            if (list == null || !list.Any())
            {
                return false;
            }
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    connection.Delete(null, TableName, transaction: trans);
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


        public bool BatchReplace(int baseId, List<ProductStock> list)
        {
            if (list == null || !list.Any())
            {
                return false;
            }
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    connection.Delete(new {baseId}, TableName, transaction: trans);
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
