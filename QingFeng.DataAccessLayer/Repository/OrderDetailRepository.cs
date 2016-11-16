using QingFeng.Common.Dapper;
using QingFeng.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>
    {
        const string TableName = "orderDetail";
        public OrderDetailRepository() : base(TableName) { }

        public IEnumerable<OrderDetail> GetBatchOrderDetails(params int[] orderId)
        {
            if (orderId == null || !orderId.Any())
            {
                return new List<OrderDetail>();
            }

            var additional = $"AND orderId IN ('{string.Join("','", orderId)}')";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<OrderDetail>(null, TableName, buildWhereSql);
            }
        }
    }
}
