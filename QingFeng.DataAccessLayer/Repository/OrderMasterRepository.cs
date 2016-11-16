using QingFeng.Models;
using System;
using System.Collections.Generic;
using QingFeng.Common.Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderMasterRepository : RepositoryBase<OrderMaster>
    {
        const string TableName = "orderMaster";

        public OrderMasterRepository() : base(TableName) { }


        public bool CreateOrder(OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            using (var connection = GetWriteConnection)
            {
                connection.Open();
                var trans = connection.BeginTransaction();
                try
                {
                    connection.Insert(orderMaster, "orderMaster", trans);
                    foreach (var item in orderDetails)
                    {
                        connection.Insert(item, "orderDetail", trans);
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

        public IEnumerable<OrderMaster> SearchOrder(dynamic condition, int page, int pageSize, out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(condition.keyWords)
              ? string.Empty
              : "AND (orderId LIKE @keyWords OR orderNo LIKE @keyWords OR contactName LIKE @keyWords OR contactPhone LIKE @keyWords)";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            var conditionObj = condition as object;
            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<OrderMaster>(conditionObj, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }
    }
}
