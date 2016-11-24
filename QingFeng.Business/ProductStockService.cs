using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace QingFeng.Business
{
    public class ProductStockService
    {
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductStockRepository _productStockRepository = new ProductStockRepository();

        public int CreateProductStock(int productId, List<KeyValuePair<int, string>> sizeSku)
        {
            var product = _productRepository.Get(new {productId});
            if (product == null)
            {
                return 0;
            }
            var productStockList = _productStockRepository.GetList(new {productId}).ToList();
            var addCount = 0;
            using (var trans = new TransactionScope())
            {
                foreach (var sku in sizeSku.OrderBy(t => t.Key))
                {
                    if (productStockList.Exists(t => t.SkuId == sku.Key)) continue;
                    var productStock = new ProductStock()
                    {
                        BaseId = product.BaseId,
                        ProductId = product.ProductId,
                        SkuId = sku.Key,
                        SkuName = sku.Value,
                        UpdateDate = DateTime.Now
                    };
                    if (_productStockRepository.Insert(productStock) > 0)
                    {
                        addCount++;
                    }
                }
                trans.Complete();
            }
            return addCount;
        }

        public bool UpdateProductStock(object model, object condition)
        {
            return _productStockRepository.Update(model, condition);
        }

        public IEnumerable<ProductStock> GetList(object condition)
        {
            return _productStockRepository.GetList(condition);
        }

        public bool SetProductStock(int productId, List<KeyValuePair<int, int>> sizeSku)
        {
            return false;
        }
    }
}
