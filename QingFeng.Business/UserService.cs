using System;
using System.Collections.Generic;
using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.Common.Utilities;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business
{
    public class UserService
    {
        private const string PassWordSplitString = "#agent@com";
        private readonly UserInfoRepository _userInfoRepository = new UserInfoRepository();

        public IEnumerable<UserInfo> GetPageList(int page, int pageSize, out int totalItem)
        {
            return _userInfoRepository.GetPageList(new { }, "", page, pageSize, out totalItem);
        }

        //注册用户
        public int RegisterUser(UserInfo model)
        {
            if (_userInfoRepository.Count(new { model.UserName }) > 0)
            {
                return 2;
            }

            model.UserRole = AgentEnums.UserRole.StoreUser;
            model.Salt = StringExtensions.GetRandomString();
            model.PassWord = string.Concat(model.UserName, PassWordSplitString, model.PassWord).Hmacsha1(model.Salt);
            model.CreateDate = DateTime.Now;

            return _userInfoRepository.Insert(model) > 0 ? 1 : 0;
        }


        public bool UpdateUserInfo(UserInfo model, string newPwd = "")
        {
            if (!string.IsNullOrWhiteSpace(newPwd))
            {
                model.PassWord = string.Concat(model.UserName, PassWordSplitString, newPwd).Hmacsha1(model.Salt);
            }

            //var obj = DataContractExtensions.SimpleModel(model, false, "UserId");

            return _userInfoRepository.Update(model, new { model.UserId });
        }

        public UserInfo Login(string userName, string passWord, out bool isPass)
        {
            var user = _userInfoRepository.Get(new { userName });

            if (user == null)
            {
                isPass = false;
                return null;
            }

            isPass =
                string.Concat(user.UserName, PassWordSplitString, passWord).Hmacsha1(user.Salt).Equals(user.PassWord);

            return user;
        }

        public UserInfo GetUserInfo(object condition)
        {
            var userInfo = _userInfoRepository.Get(condition);

            if (userInfo != null)
            {
                userInfo.StoreInfo = new StoreService().GetStoreInfo(new { masterUserId = userInfo.UserId });
            }

            return userInfo;
        }
    }
}

