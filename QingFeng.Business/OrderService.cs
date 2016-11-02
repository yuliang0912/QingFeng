using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Collections.Generic;

namespace QingFeng.Business
{
    public class OrderService
    {
        private readonly OrderMasterRepository _orderMaster = new OrderMasterRepository();
        private readonly OrderDetailRepository _orderDetail = new OrderDetailRepository();


        public bool CreateOrder(OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            return _orderMaster.CreateOrder(orderMaster, orderDetails);
        }

        public bool UpdateOrder(object model, object condition)
        {
            return _orderMaster.Update(model, condition);
        }
    }
}
