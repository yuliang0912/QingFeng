﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 用户单元测试

            UserUnitTest.registerTest();
            UserUnitTest.loginTest();
            UserUnitTest.GetUserInfoTest();
            #endregion

            Console.ReadKey();
        }
    }
}
