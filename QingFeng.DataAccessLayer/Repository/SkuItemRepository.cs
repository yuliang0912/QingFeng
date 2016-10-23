using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class SkuItemRepository : RepositoryBase<SkuItem>
    {
        public SkuItemRepository() : base("qingfeng", "skuItems")
        {
        }
    }
}
