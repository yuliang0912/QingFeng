using System;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common.Extensions;
using QingFeng.Common;

namespace QingFeng.Business
{
    public class OrderService
    {
        private readonly OrderMasterRepository _orderMaster = new OrderMasterRepository();
        private readonly OrderDetailRepository _orderDetail = new OrderDetailRepository();


        public bool CreateOrder(OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            if (orderMaster == null || orderDetails == null || !orderDetails.Any())
            {
                return false;
            }

            var orderId = GuidConvert.ToUniqueId();

            orderMaster.OrderStatus = AgentEnums.MasterOrderStatus.WaitPay;
            orderMaster.OrderId = orderId;
            orderMaster.CreateDate = DateTime.Now;
            orderMaster.PayStatus = 1;
            orderMaster.PayDate = new DateTime(1970, 1, 1);
            orderDetails.ForEach(t =>
            {
                t.OrderId = orderId;
                t.OrderSatus = AgentEnums.OrderDetailStatus.WaitDeliverGoods;
            });

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

        public IEnumerable<OrderMaster> SearchOrderList(int storeId, int orderStatus, DateTime beginDate,
            DateTime endDate,
            string keyWords, int page,
            int pageSize, out int totalItem)
        {
            var list =
                _orderMaster.SearchOrder(storeId, orderStatus, beginDate, endDate, keyWords, page, pageSize,
                    out totalItem).ToList();

            if (list.Any()) return list;
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

            return list;
        }
    }
}
