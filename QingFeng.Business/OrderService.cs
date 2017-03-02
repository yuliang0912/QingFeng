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
        private readonly ProductBaseRepository _productBase =new ProductBaseRepository();
        private readonly LogisticsRepository _logistics = new LogisticsRepository();
        private readonly OrderMasterRepository _orderMaster = new OrderMasterRepository();
        private readonly OrderDetailRepository _orderDetail = new OrderDetailRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();
        private readonly OrderLogsRepository _orderLogs = new OrderLogsRepository();


        public bool CreateOrder(UserInfo user, OrderMaster orderMaster, List<OrderDetail> orderDetails)
        {
            if (orderMaster == null || orderDetails == null || !orderDetails.Any())
            {
                return false;
            }

            var productList = _product.GetProductListByIds(orderDetails.Select(t => t.ProductId).ToArray())
                .ToDictionary(c => c.ProductId, c => c);

            var baseProductList = _productBase.GetListByIds(productList.Select(t => t.Value.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            var skuIds = productList.Select(t => t.Value.ColorId).ToList();
            skuIds.AddRange(orderDetails.Select(t => t.SkuId).ToList());

            var skuList = _skuItemRepository.GetListByIds(skuIds.ToArray())
                .ToDictionary(c => c.SkuId, c => c.SkuName);

            var orderId = GuidConvert.ToUniqueId();

            var remark = string.Empty;

            orderDetails.ForEach(t =>
            {
                var product = productList[t.ProductId];
                t.Quantity = t.Quantity < 1 ? 1 : t.Quantity;
                t.BaseId = product.BaseId;
                t.BaseNo = baseProductList[product.BaseId].BaseNo;
                t.BaseName = baseProductList[product.BaseId].BaseName;
                t.ProductNo = product.ProductNo;
                t.ProductName = product.ProductName;
                t.Price = product.ActualPrice;
                t.OrderId = orderId;
                t.OrderNo = orderMaster.OrderNo;
                t.Remark = (t.Remark ?? string.Empty).CutString(120);
                t.OrderStatus = AgentEnums.OrderDetailStatus.待发货;
                t.Amount = t.Price*t.Quantity;
                t.SkuName = skuList[t.SkuId];
                remark += t.SkuName + "  ";
            });

            orderMaster.StoreName = user.StoreList.FirstOrDefault(t => t.StoreId == orderMaster.StoreId)?.StoreName ??
                                    string.Empty;
            orderMaster.Remark = (orderMaster.Remark ?? string.Empty).CutString(500);
            orderMaster.OrderStatus = AgentEnums.MasterOrderStatus.待支付;
            orderMaster.OrderId = orderId;
            orderMaster.CreateDate = DateTime.Now;
            orderMaster.PayStatus = 1;
            orderMaster.PayDate = new DateTime(1970, 1, 1);
            orderMaster.OrderDetailCount = orderDetails.Count;
            orderMaster.OrderAmount = orderDetails.Sum(t => t.Amount);

            var result = _orderMaster.CreateOrder(orderMaster, orderDetails);

            if (result)
            {
                _orderLogs.Insert(new OrderLogs()
                {
                    UserId = user.UserId,
                    OrderId = orderId,
                    UserName = user.UserName,
                    Title = "添加订单",
                    Content = string.IsNullOrWhiteSpace(orderMaster.Remark) ? remark : orderMaster.Remark,
                    CreateDate = DateTime.Now
                });
            }

            return result;
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="flowIds"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendDeliverGoods(UserInfo user, OrderMaster orderInfo, List<int> flowIds, LogisticsInfo model)
        {
            if (model == null || flowIds == null || !flowIds.Any())
            {
                return false;
            }

            model.OrderId = orderInfo.OrderId;
            model.UpdateDate = DateTime.Now;
            model.CreateDate = DateTime.Now;
            model.FlowIds = string.Join(",", flowIds);
            model.Status = 0;

            using (var trans = new TransactionScope())
            {
                _orderDetail.BatchUpdateOrderStatus(orderInfo.OrderId, flowIds,
                    AgentEnums.OrderDetailStatus.已发货);
                _logistics.Insert(model); //物流
                var waitDeliverGoodsCount =
                    _orderDetail.Count(
                        new {orderInfo.OrderId, orderStatus = AgentEnums.OrderDetailStatus.待发货});
                _orderMaster.Update(
                    new
                    {
                        orderStatus =
                            waitDeliverGoodsCount == 0
                                ? AgentEnums.MasterOrderStatus.已完成
                                : AgentEnums.MasterOrderStatus.进行中
                    }, new {orderInfo.OrderId});
                _orderLogs.Insert(new OrderLogs()
                {
                    OrderId = orderInfo.OrderId,
                    Title = "发货",
                    Content = model.CompanyName + ",运单号:" + model.OddNumber,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    CreateDate = DateTime.Now
                });
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

        public IEnumerable<OrderMaster> SearchOrderList(int userId, int storeId, int orderStatus, DateTime beginDate,
            DateTime endDate,
            string keyWords, int page,
            int pageSize, out int totalItem)
        {
            var list =
                _orderMaster.SearchOrder(userId, storeId, orderStatus, beginDate, endDate, keyWords, page, pageSize,
                    out totalItem).ToList();

            if (!list.Any()) return list;
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
