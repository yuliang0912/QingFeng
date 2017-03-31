using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace QingFeng.Business
{
    public class PayOrderService : Singleton<PayOrderService>
    {
        private PayOrderService()
        {
        }

        private readonly PayOrderRepository _payOrderRepository = new PayOrderRepository();
        private readonly OrderMasterRepository _orderMasterRepository = new OrderMasterRepository();

        public PayOrder CreatePayOrder(OrderMaster orderInfo)
        {
            var model = new PayOrder()
            {
                PayNo = "M" + GuidConvert.ToUniqueId(),
                OrderNo = orderInfo.OrderNo,
                OrderId = orderInfo.OrderId,
                OutsideId = string.Empty,
                PayType = AgentEnums.PayType.智付,
                ActualPrice = orderInfo.OrderAmount,
                CounterFee = orderInfo.OrderAmount*0.01m,
                UserId = orderInfo.UserId,
                PayStatus = AgentEnums.PayStatus.待支付,
                VerifyStatus = AgentEnums.VerifyStatus.未审核,
                CreateDate = DateTime.Now,
                PayDate = new DateTime(2000, 1, 1),
                VerifyDate = new DateTime(2000, 1, 1),
            };

            return _payOrderRepository.Insert(model) > 0 ? model : null;
        }


        public IEnumerable<PayOrder> SearchPayOrder(int payStatus, int verifyStatus, DateTime beginDate, DateTime endDate,
            string keyWords,
            int page,
            int pageSize, out int totalItem)
        {
            return _payOrderRepository.SearchPayOrder(payStatus, verifyStatus, beginDate, endDate, keyWords, page, pageSize,
                out totalItem);
        }


        public bool Update(object model, object condition)
        {
            return _payOrderRepository.Update(model, condition);
        }

        public PayOrder Get(object condition)
        {
            return _payOrderRepository.Get(condition);
        }

        public bool SetPayed(PayOrder model, string tradeTime, string tradeNo, string tradeStatus)
        {
            using (var trans = new TransactionScope())
            {
                _payOrderRepository.Update(new
                {
                    OutsideId = tradeNo,
                    PayType = AgentEnums.PayType.智付,
                    PayStatus = tradeStatus == "SUCCESS" ? AgentEnums.PayStatus.支付成功 : AgentEnums.PayStatus.支付失败,
                    PayDate = Convert.ToDateTime(tradeTime)
                }, new {model.PayNo});
                _orderMasterRepository.Update(new
                {
                    OrderStatus = AgentEnums.MasterOrderStatus.待发货,
                    PayNo = model.PayNo
                }, new {model.OrderId});
                trans.Complete();
            }
            return true;
        }
    }
}
