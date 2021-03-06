﻿using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.Common.Utilities;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Transactions;

namespace QingFeng.Business
{
    public class UserService
    {
        private const string PassWordSplitString = "#agent@com";
        private readonly UserInfoRepository _userInfoRepository = new UserInfoRepository();
        private readonly StoreRepository _storeRepository =new StoreRepository();

        public IEnumerable<UserInfo> GetPageList(object condition, int page, int pageSize, out int totalItem)
        {
            var list = _userInfoRepository.GetPageList(condition, "", page, pageSize, out totalItem);

            if (list.Any())
            {
                var storeInfos =
                    _storeRepository.GetBatchStoreInfos(list.Select(t => t.UserId).ToArray())
                        .GroupBy(t => t.MasterUserId, c => c)
                        .ToDictionary(c => c.Key, c => c.ToList());

                list.ToList().ForEach(t =>
                {
                    if (storeInfos.ContainsKey(t.UserId))
                    {
                        t.StoreList = storeInfos[t.UserId];
                    }
                });
            }

            return list;
        }

        //注册用户
        public int RegisterUser(UserInfo model)
        {
            if (_userInfoRepository.Count(new {model.UserName}) > 0)
            {
                return 2;
            }

            model.Avatar = model.Avatar ?? "/content/agent/img/user.jpg";
            model.NickName = model.NickName ?? model.UserName;
            model.UserRole = AgentEnums.UserRole.StoreUser;
            model.Salt = StringExtensions.GetRandomString();

            model.PassWord =
                string.Concat(model.UserName, PassWordSplitString, model.UserRole.GetHashCode(), model.PassWord)
                    .Hmacsha1(model.Salt);
            model.CreateDate = DateTime.Now;

            return _userInfoRepository.Insert(model) > 0 ? 1 : 0;
        }

        public int UpdatePassWord(int userId, string password)
        {
            var userInfo = _userInfoRepository.Get(new {userId});

            if (userInfo == null)
            {
                return 0;
            }

            userInfo.Salt = StringExtensions.GetRandomString();

            var newPassWord =
                string.Concat(userInfo.UserName, PassWordSplitString, userInfo.UserRole.GetHashCode(), password)
                    .Hmacsha1(userInfo.Salt);

            return _userInfoRepository.Update(new {passWord = newPassWord, userInfo.Salt}, new {userId}) ? 1 : 0;
        }

        public int UpdatePassWord(UserInfo user, string oldPwd, string newPwd)
        {
            if (!string.Concat(user.UserName, PassWordSplitString, user.UserRole.GetHashCode(), oldPwd)
                .Hmacsha1(user.Salt).Equals(user.PassWord))
            {
                return 2;
            }

            var newPassWord =
                string.Concat(user.UserName, PassWordSplitString, user.UserRole.GetHashCode(), newPwd)
                    .Hmacsha1(user.Salt);

            return _userInfoRepository.Update(new {passWord = newPassWord}, new {user.UserId}) ? 1 : 0;
        }

        public bool UpdateUserInfo(UserInfo model)
        {
            if (model == null)
            {
                return false;
            }
            var userInfo = _userInfoRepository.Get(new {model.UserId});
            if (userInfo == null)
            {
                return false;
            }

            model.NickName = model.NickName ?? userInfo.UserName;
            return _userInfoRepository.Update(new {model.NickName}, new {model.UserId});
        }

        public bool DelOrRecoveryStatus(int userId)
        {
            var userInfo = _userInfoRepository.Get(new {userId, userRole = AgentEnums.UserRole.StoreUser.GetHashCode()});
            if (userInfo == null)
            {
                return false;
            }
            return _userInfoRepository.Update(new {status = userInfo.Status == 0 ? 1 : 0}, new {userId});
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
                string.Concat(user.UserName, PassWordSplitString, user.UserRole.GetHashCode(), passWord).Hmacsha1(user.Salt).Equals(user.PassWord);
            return user;
        }

        public UserInfo GetUserInfo(object condition)
        {
            var userInfo = _userInfoRepository.Get(condition);

            if (userInfo != null)
            {
                userInfo.StoreList = _storeRepository.GetBatchStoreInfos(userInfo.UserId).ToList();
            }

            return userInfo;
        }

        public int Count(object condition)
        {
            return _userInfoRepository.Count(condition);
        }
    }
}

