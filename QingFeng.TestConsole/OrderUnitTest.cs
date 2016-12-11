using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Common.Extensions;
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
                OrderNo = "FN" + GuidConvert.ToString16(),
                UserId = user.UserId,
                StoreId = user.StoreList.First().StoreId,
                ContactName = "余亮",
                ContactPhone = "18923803593",
                Address = "民治大道" + new Random().Next(1, 9) + "栋",
                PostCode = "518" + new Random().Next(101, 999),
                AreaCode = 4403,
                OrderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        ProductId = 10000,
                        Quantity = new Random().Next(1, 9),
                        SkuId = new Random().Next(4, 9) //码数
                    },
                    new OrderDetail()
                    {
                        ProductId = 10001,
                        Quantity = new Random().Next(1, 9),
                        SkuId = new Random().Next(4, 9)
                    }
                },
            };

            if (new Random().Next(0, 3) == 0)
            {
                order.OrderDetails = order.OrderDetails.Skip(1).ToList();
            }

            var result = OrderService.CreateOrder(user, order, order.OrderDetails.ToList());

            Console.WriteLine("创建订单测试结果:{0}", result);
        }


        public static void SearchOrderListTest()
        {
            int totalItem, storeId = 6, orderStatus = 3;
            var keyWords = "";

            var list = OrderService.SearchOrderList(0,storeId, orderStatus, DateTime.MinValue, DateTime.Now, keyWords, 1, 10,
                out totalItem);

            var result = new ApiPageList<OrderMaster>()
            {
                Page = 1,
                PageSize = 10,
                TotalCount = totalItem,
                PageList = list
            };
            Console.WriteLine("搜索订单测试结果:");
            Console.WriteLine(JsonHelper.Encode(result));
        }
    }
}
