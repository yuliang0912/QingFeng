﻿using System;
using System.Collections.Generic;
using QingFeng.Common.Extensions;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business
{
    public class StoreService
    {
        private readonly StoreRepository _storeRepository = new StoreRepository();

        public int CreateStore(StoreInfo model)
        {
            return _storeRepository.Insert(model);
        }

        public bool UpdateStoreInfo(object model, object condition)
        {
            return _storeRepository.Update(model, condition);
        }

        public StoreInfo GetStoreInfo(object condition)
        {
            return _storeRepository.Get(condition);
        }

        public IEnumerable<StoreInfo> GetList(int userId)
        {
            return _storeRepository.GetBatchStoreInfos(userId);
        }

        public bool AddStore(int userId, string homeUrl)
        {
            var count = _storeRepository.Count(new {masterUserId = userId});
            if (count > 98)
            {
                return false;
            }

            var storeInfo = new StoreInfo()
            {
                StoreName = "F" + StringExtensions.FillZeroNumber(count + 1, 2),
                MasterUserId = userId,
                CreateDate = DateTime.Now,
                HomeUrl = homeUrl ?? string.Empty,
                Status = 0
            };

            return _storeRepository.Insert(storeInfo) > 0;
        }

        public bool UpdateStoreStatus(int storeId, int status)
        {
            return _storeRepository.Update(new {status}, new { storeId });
        }
    }
}
