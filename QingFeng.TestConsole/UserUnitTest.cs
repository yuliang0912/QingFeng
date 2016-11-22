using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.TestConsole
{
    public static class UserUnitTest
    {
        private static UserService _userService = new UserService();

        public static void registerTest()
        {
            var model = new UserInfo()
            {
                Salt = StringExtensions.GetRandomString(),
                PassWord = "123456",
                UserName = "admin",
                NickName = "admin",
                Avatar = string.Empty,
                StoreInfo = new StoreInfo()
                {
                    StoreName = "my store",
                    HomeUrl = string.Empty
                }
            };

            var result = _userService.RegisterUser(model);
            Console.WriteLine("注册用户测试结果:{0}", result);
        }

        public static void loginTest()
        {
            var isPass = false;
            _userService.Login("admin", "123456", out isPass);
            Console.WriteLine("登录测试结果:{0}", isPass);
        }

        public static void GetUserInfoTest()
        {
            var model = _userService.GetUserInfo(new { userName = "admin" });
            Console.WriteLine("查询用户信息结果:{0}", JsonHelper.Encode(model));
        }
    }
}
