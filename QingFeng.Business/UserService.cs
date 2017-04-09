﻿using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Common;
using QingFeng.Common.Extensions;
using QingFeng.Common.Utilities;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Models.DTO;

namespace QingFeng.Business
{
    public class UserService : Singleton<UserService>
    {
        private UserService()
        {
        }

        private const string PassWordSplitString = "#agent@com";
        private readonly StoreRepository _storeRepository = new StoreRepository();
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly UserInfoRepository _userInfoRepository = new UserInfoRepository();
        private readonly UserProductPriceRepository _userProductPriceRepository = new UserProductPriceRepository();

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
            model.NickName = model.NickName ?? string.Empty;
            model.Salt = StringExtensions.GetRandomString();
            model.LastLoginDate = new DateTime(2000, 1, 1);
            model.LastLoginIp = "-";
            model.Phone = model.Phone ?? string.Empty;
            model.Email = model.Email ?? string.Empty;

            model.PassWord =
                string.Concat(model.UserName, PassWordSplitString, model.UserRole.GetHashCode(), model.PassWord)
                    .Hmacsha1(model.Salt);
            model.CreateDate = DateTime.Now;

            return _userInfoRepository.Insert(model) > 0 ? 1 : 0;
        }

        public bool UpdatePassWord(UserInfo userInfo, string password)
        {
            userInfo.Salt = StringExtensions.GetRandomString();

            var newPassWord =
                string.Concat(userInfo.UserName, PassWordSplitString, userInfo.UserRole.GetHashCode(), password)
                    .Hmacsha1(userInfo.Salt);

            return _userInfoRepository.Update(new {passWord = newPassWord, userInfo.Salt}, new {userInfo.UserId});
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
            model.Phone = model.Phone ?? string.Empty;
            model.Email = model.Email ?? string.Empty;
            model.NickName = model.NickName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(model.PassWord))
            {
                var newPassWord =
                    string.Concat(model.UserName, PassWordSplitString, model.UserRole.GetHashCode(),
                        model.PassWord)
                        .Hmacsha1(model.Salt);
                model.PassWord = newPassWord;
            }

            return _userInfoRepository.Update(model, new {model.UserId});
        }

        public bool Update(object model, object conditon)
        {
            return _userInfoRepository.Update(model, conditon);
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

        public UserInfo Login(string userName, string passWord, string ip, out bool isPass)
        {
            var user = _userInfoRepository.Get(new {userName});

            if (user == null)
            {
                isPass = false;
                return null;
            }

            isPass =
                string.Concat(user.UserName, PassWordSplitString, user.UserRole.GetHashCode(), passWord)
                    .Hmacsha1(user.Salt)
                    .Equals(user.PassWord);

            if (isPass)
            {
                _userInfoRepository.Update(new
                {
                    lastLoginIp = ip,
                    lastLoginDate = DateTime.Now,
                }, new {user.UserId});
            }

            return user;
        }

        public UserInfo GetUserInfo(object condition)
        {
            var userInfo = _userInfoRepository.Get(condition);

            if (userInfo == null)
            {
                return null;
            }

            userInfo.StoreList = userInfo.UserRole == AgentEnums.UserRole.StoreUser
                ? _storeRepository.GetBatchStoreInfos(userInfo.UserId).ToList()
                : _storeRepository.GetList(new {status = 0, isSelfSupport = 1}).ToList();

            return userInfo;
        }

        public int Count(object condition)
        {
            return _userInfoRepository.Count(condition);
        }

        public IEnumerable<UserInfo> GetList(object condition)
        {
            return _userInfoRepository.GetList(condition);
        }

        public IEnumerable<UserProductPrice> GetUserPrice(int userId, int brandId, params int[] baseId)
        {
            return _userProductPriceRepository.GetListByBaseIds(userId, baseId);
        }

        public int ResetUserPrice(int userId, int brandId, List<UserPriceExcelDto> list)
        {
            if (list == null || !list.Any())
            {
                return 0;
            }

            var productList =
                _productRepository.GetProductListByIds(list.Select(t => t.ProductId).ToArray())
                    .ToDictionary(c => c.ProductId, c => c);

            if (!productList.Any())
            {
                return 0;
            }

            var addList = new List<UserProductPrice>();
            foreach (var item in list)
            {
                if (!productList.ContainsKey(item.ProductId))
                {
                    continue;
                }
                var product = productList[item.ProductId];
                if (product.BaseId == item.BaseId && product.BaseNo == item.BaseNo.Trim() &&
                    product.ProductNo == item.ProductNo.Trim())
                {
                    addList.Add(new UserProductPrice()
                    {
                        BaseId = product.BaseId,
                        BrandId = (AgentEnums.Brand) brandId,
                        ProductId = product.ProductId,
                        UserId = userId,
                        OriginalPrice = item.OriginalPrice,
                        ActualPrice = item.ActualPrice,
                        UpdateDate = DateTime.Now
                    });
                }
            }

            var isSuccess = _userProductPriceRepository.BatchInsert(userId, brandId, addList);

            return isSuccess ? addList.Count() : 0;
        }

        public bool ResetUserPrice(int userId, int baseId, List<UserProductPrice> list)
        {
            return _userProductPriceRepository.BatchInsert(userId, baseId, list);
        }

        public IEnumerable<UserInfo> Search(AgentEnums.UserRole userRole, string keyWords)
        {
            return _userInfoRepository.Search(keyWords, userRole);
        }
    }
}

