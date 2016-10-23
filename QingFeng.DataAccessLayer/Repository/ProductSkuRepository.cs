using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductSkuRepository : RepositoryBase<ProductSku>
    {
        public ProductSkuRepository() : base("qingfeng", "productSkus")
        {
        }
    }
}
