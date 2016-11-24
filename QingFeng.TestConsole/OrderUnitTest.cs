using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
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
                OrderNo = "515182254518",
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

            var result = OrderService.CreateOrder(user, order, order.OrderDetails.ToList());

            Console.WriteLine("创建订单测试结果:{0}", result);
        }


        public static void SearchOrderListTest()
        {
            int totalItem, storeId = 5, orderStatus = 3;
            var keyWords = "18923803594";

            var list = OrderService.SearchOrderList(storeId, orderStatus, DateTime.MinValue, DateTime.Now, keyWords, 1, 10,
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
