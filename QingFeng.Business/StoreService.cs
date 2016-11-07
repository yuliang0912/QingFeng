using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
