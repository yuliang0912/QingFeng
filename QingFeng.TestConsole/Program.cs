using QingFeng.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common.Extensions;

namespace QingFeng.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 用户单元测试

            //UserUnitTest.RegisterTest();
            //UserUnitTest.loginTest();
            //UserUnitTest.GetUserInfoTest();
            //ProductUnitTest.CreateBaseProductTest();
            //ProductUnitTest.CreateProductTest();
            //ProductUnitTest.SearchProductTest();
            //ProductUnitTest.SearchProductStockTest();
            //OrderUnitTest.CreateOrder();
            OrderUnitTest.SearchOrderListTest();
            //ProductUnitTest.CreateProductStockTest();

            #endregion

            Console.ReadKey();
        }
    }
}
