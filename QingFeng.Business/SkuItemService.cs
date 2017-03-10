using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business
{
    public class SkuItemService : Singleton<SkuItemService>
    {
        private SkuItemService() { }

        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();

        public IEnumerable<SkuItem> GetList()
        {
            return _skuItemRepository.GetList(null);
        }

        public IEnumerable<SkuItem> GetList(AgentEnums.SkuType skuType)
        {
            return _skuItemRepository.GetList(new { skuType = skuType.GetHashCode() });
        }

        public bool Update(object model, object condition)
        {
            return _skuItemRepository.Update(model, condition);
        }

        public bool IsExists(string skuName, AgentEnums.SkuType skuType)
        {
            return _skuItemRepository.Count(new { skuName = skuName.Trim(), skuType = skuType.GetHashCode() }) > 0;
        }

        public int AddSkuItem(SkuItem model)
        {
            if (IsExists(model.SkuName, model.SkuType))
            {
                return 2;
            }
            return _skuItemRepository.Insert(model) > 0 ? 1 : 0;
        }
    }
}
