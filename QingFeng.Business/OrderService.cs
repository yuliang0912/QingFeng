using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<OrderMaster> SearchOrderList(object condition, int page, int pageSize, out int totalItem)
        {
            var list = _orderMaster.SearchOrder(condition, page, pageSize, out totalItem).ToList();

            if (!list.Any())
            {
                var orderDetails = _orderDetail.GetBatchOrderDetails(list.Select(t => t.OrderId).ToArray())
                    .GroupBy(t => t.OrderId)
                    .ToDictionary(c => c.Key, c => c);

                list.ForEach(t =>
                {
                    if (orderDetails.ContainsKey(t.OrderId))
                    {
                        t.OrderDetails = orderDetails[t.OrderId];
                    }
                });
            }

            return list;
        }
    }
}
