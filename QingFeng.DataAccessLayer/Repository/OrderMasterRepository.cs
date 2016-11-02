using QingFeng.Models;
using System;
using System.Collections.Generic;
using QingFeng.Common.Dapper;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderMasterRepository : RepositoryBase<OrderMaster>
    {
        public OrderMasterRepository() : base("orderMaster") { }


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
    }
}
