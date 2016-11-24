using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Business;
using QingFeng.Models;

namespace QingFeng.TestConsole
{
    public static class OrderUnitTest
    {
        private static readonly OrderService OrderService = new OrderService();
        private static readonly UserService UserService = new UserService();

        public static void CreateOrder()
        {
            var user = UserService.GetUserInfo(new {userName = "admin"});
            var order = new OrderMaster
            {
                OrderNo = "45123513893618",
                UserId = user.UserId,
                StoreId = user.StoreInfo.StoreId,
                ContactName = "余亮",
                ContactPhone = "18923803593",
                Address = "民治大道",
                PostCode = "518000",
                AreaCode = 4403,
                OrderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        ProductId = 7,
                        Quantity = 2,
                        SkuId = 5 //码数
                    },
                    new OrderDetail()
                    {
                        ProductId = 10,
                        Quantity = 1,
                        SkuId = 6
                    }
                },
            };

            var result = OrderService.CreateOrder(order, order.OrderDetails.ToList());

            Console.WriteLine("创建订单测试结果:{0}", result);
        }
    }
}
