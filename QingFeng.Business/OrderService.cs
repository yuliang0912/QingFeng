using System;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QingFeng.Business
{
    public class OrderService
    {
        private readonly OrderMasterRepository _orderMaster = new OrderMasterRepository();
        private readonly OrderDetailRepository _orderDetail = new OrderDetailRepository();


        public bool CreateOrder(OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            int orderId = 0;

            orderMaster.OrderId = orderId;
            orderMaster.CreateDate = DateTime.Now;
            orderDetails.ForEach(t => t.OrderId = orderId);

            return _orderMaster.CreateOrder(orderMaster, orderDetails);
        }

        public bool UpdateOrder(object model, object condition)
        {
            return _orderMaster.Update(model, condition);
        }

        public OrderMaster Get(object conditon)
        {
            var model = _orderMaster.Get(conditon);
            if (null != model)
            {
                model.OrderDetails = _orderDetail.GetList(new {model.OrderNo});
            }
            return model;
        }

        public bool IsExists(string orderNo)
        {
            return _orderMaster.Count(new {orderNo}) > 0;
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
