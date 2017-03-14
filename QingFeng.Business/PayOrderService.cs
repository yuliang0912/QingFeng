using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System;

namespace QingFeng.Business
{
    class PayOrderService : Singleton<PayOrderService>
    {
        private PayOrderService() { }


        private readonly PayOrderRepository _ayOrderRepository = new PayOrderRepository();

        public PayOrder CreatePayOrder(OrderMaster orderInfo)
        {
            var model = new PayOrder()
            {
                PayNo = "M" + GuidConvert.ToUniqueId(),
                OrderNo = orderInfo.OrderNo,
                OrderId = orderInfo.OrderId,
                PayType = AgentEnums.PayType.智付,
                ActualPrice = orderInfo.OrderAmount,
                UserId = orderInfo.UserId,
                Status = 0,
                VerifyStatus = 0,
                CreateDate = DateTime.Now
            };

            return _ayOrderRepository.Insert(model) > 0 ? model : null;
        }
    }
}
