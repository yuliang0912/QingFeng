using QingFeng.Models;
using System;
using System.Collections.Generic;
using QingFeng.Common.Dapper;
using QingFeng.Common.Extensions;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderMasterRepository : RepositoryBase<OrderMaster>
    {
        const string TableName = "orderMaster";

        public OrderMasterRepository() : base(TableName)
        {
        }


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

        public IEnumerable<OrderMaster> SearchOrder(int storeId, int orderStatus, DateTime beginDate, DateTime endDate,
            string keyWords,
            int page,
            int pageSize, out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(keyWords)
                ? string.Empty
                : "AND (orderId LIKE @keyWords OR orderNo LIKE @keyWords OR contactName LIKE @keyWords OR contactPhone LIKE @keyWords) ";

            additional += "AND CreateDate Between @beginDate AND @endDate ";

            if (storeId > 0)
            {
                additional += $"AND storeId = {storeId} ";
            }
            if (orderStatus > 0)
            {
                additional += $"AND orderStatus = {orderStatus} ";
            }

            var condition = new
            {
                beginDate,
                endDate,
                storeId,
                keyWords = string.IsNullOrWhiteSpace(keyWords) ? string.Empty : keyWords.FormatSqlLikeString(),
                orderStatus
            };

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords", "beginDate", "endDate");

            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<OrderMaster>(condition, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }
    }
}
