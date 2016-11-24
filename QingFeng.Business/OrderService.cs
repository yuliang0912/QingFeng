using System;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using QingFeng.Common.Extensions;
using QingFeng.Common;

namespace QingFeng.Business
{
    public class OrderService
    {
        private readonly ProductRepository _product = new ProductRepository();
        private readonly LogisticsRepository _logistics = new LogisticsRepository();
        private readonly OrderMasterRepository _orderMaster = new OrderMasterRepository();
        private readonly OrderDetailRepository _orderDetail = new OrderDetailRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();


        public bool CreateOrder(OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            if (orderMaster == null || orderDetails == null || !orderDetails.Any())
            {
                return false;
            }

            var productList = _product.GetProductListByIds(orderDetails.Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            if (productList.Count != orderDetails.Count)
            {
                return false;
            }

            var skuList = _skuItemRepository.GetListByIds(orderDetails.Select(t => t.SkuId).ToArray())
                .ToDictionary(c => c.SkuId, c => c.SkuName);

            if (skuList.Count != orderDetails.Select(t => t.SkuId).Distinct().Count())
            {
                return false;
            }

            var orderId = GuidConvert.ToUniqueId();

            orderDetails.ForEach(t =>
            {
                var product = productList[t.ProductId];
                t.Quantity = t.Quantity < 1 ? 1 : t.Quantity;
                t.BaseId = product.BaseId;
                t.ProductName = product.ProductName;
                t.ImgUrl = string.IsNullOrWhiteSpace(product.ImgList)
                    ? string.Empty
                    : product.ImgList.Split(',').First();
                t.Price = product.ActualPrice;
                t.OrderId = orderId;
                t.OrderNo = orderMaster.OrderNo;
                t.Remark = (t.Remark ?? string.Empty).CutString(120);
                t.OrderSatus = AgentEnums.OrderDetailStatus.WaitDeliverGoods;
                t.Amount = t.Price*t.Quantity;
                t.SkuName = skuList[t.SkuId];
            });

            orderMaster.Remark = (orderMaster.Remark ?? string.Empty).CutString(500);
            orderMaster.OrderStatus = AgentEnums.MasterOrderStatus.WaitPay;
            orderMaster.OrderId = orderId;
            orderMaster.CreateDate = DateTime.Now;
            orderMaster.PayStatus = 1;
            orderMaster.PayDate = new DateTime(1970, 1, 1);
            orderMaster.OrderDetailCount = orderDetails.Count;
            orderMaster.OrderAmount = orderDetails.Sum(t => t.Amount);

            return _orderMaster.CreateOrder(orderMaster, orderDetails);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="flowIds"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendDeliverGoods(OrderMaster orderInfo, List<int> flowIds, LogisticsInfo model)
        {
            if (model == null || flowIds == null || !flowIds.Any())
            {
                return false;
            }

            model.CreateDate = DateTime.Now;
            model.FlowIds = string.Join(",", flowIds);

            using (var trans = new TransactionScope())
            {
                _orderDetail.BatchUpdateOrderStatus(orderInfo.OrderId, flowIds,
                    AgentEnums.OrderDetailStatus.HasDeliverGoods);
                _logistics.Insert(model); //物流
                var waitDeliverGoodsCount =
                    _orderDetail.Count(
                        new {orderInfo.OrderId, orderStatus = AgentEnums.OrderDetailStatus.WaitDeliverGoods});
                _orderMaster.Update(
                    new
                    {
                        orderStatus =
                        waitDeliverGoodsCount == 0
                            ? AgentEnums.MasterOrderStatus.Completed
                            : AgentEnums.MasterOrderStatus.Doing
                    }, new {orderInfo.OrderId});

                trans.Complete();
            }
            return true;
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
