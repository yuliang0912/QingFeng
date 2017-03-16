using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System;
using System.Collections.Generic;

namespace QingFeng.Business
{
    public class PayOrderService : Singleton<PayOrderService>
    {
        private PayOrderService() { }

        private readonly PayOrderRepository _payOrderRepository = new PayOrderRepository();

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
                CounterFee = orderInfo.OrderAmount * 0.01m,
                UserId = orderInfo.UserId,
                PayStatus = AgentEnums.PayStatus.待支付,
                VerifyStatus = AgentEnums.VerifyStatus.未审核,
                CreateDate = DateTime.Now,
                PayDate = DateTime.MinValue,
                VerifyDate = DateTime.MinValue,
            };

            return _payOrderRepository.Insert(model) > 0 ? model : null;
        }


        public IEnumerable<PayOrder> SearchPayOrder(int status, int verifyStatus, DateTime beginDate, DateTime endDate,
          string keyWords,
          int page,
          int pageSize, out int totalItem)
        {
            return _payOrderRepository.SearchPayOrder(status, verifyStatus, beginDate, endDate, keyWords, page, pageSize, out totalItem);
        }


        public bool Update(object model, object condition)
        {
            return _payOrderRepository.Update(model, condition);
        }
    }
}
