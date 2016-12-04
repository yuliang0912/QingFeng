using QingFeng.Common.Dapper;
using QingFeng.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>
    {
        const string TableName = "orderDetail";

        public OrderDetailRepository() : base(TableName)
        {
        }

        public IEnumerable<OrderDetail> GetBatchOrderDetails(params long[] orderId)
        {
            if (orderId == null || !orderId.Any())
            {
                return new List<OrderDetail>();
            }

            var additional = $"AND orderId IN ({string.Join(",", orderId)})";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional);

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<OrderDetail>(null, TableName, buildWhereSql);
            }
        }

        public bool BatchUpdateOrderStatus(long orderId, List<int> flowIds, AgentEnums.OrderDetailStatus orderStatus)
        {
            var rows = 0;
            using (var connection = GetWriteConnection)
            {
                foreach (var item in flowIds)
                {
                    rows += connection.Update(new {orderStatus}, new {orderId, flowId = item}, TableName);
                }
            }
            return rows > 0;
        }
    }
}
