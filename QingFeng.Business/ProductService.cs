using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductBaseRepository _productBaseRepository = new ProductBaseRepository();
        private readonly ProductSkuRepository _productSkuRepository = new ProductSkuRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();

        public bool AddSkuItem(SkuItem model)
        {
            return _skuItemRepository.Insert(model);
        }


    }
}
