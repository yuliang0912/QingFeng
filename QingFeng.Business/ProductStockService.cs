using QingFeng.Common;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace QingFeng.Business
{
    public class ProductStockService : Singleton<ProductStockService>
    {

        private ProductStockService() { }

        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();
        private readonly ProductStockRepository _productStockRepository = new ProductStockRepository();

        public int CreateProductStock(int productId, List<int> sizeList)
        {
            var sizeSku = _skuItemRepository.GetListByIds(sizeList.ToArray())
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName));

            if (!sizeSku.Any())
            {
                return 0;
            }

            var product = _productRepository.Get(new { productId });
            if (product == null)
            {
                return 0;
            }
            var productStockList = _productStockRepository.GetList(new { productId }).ToList();


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

        public ProductStock Get(object condition)
        {
            return _productStockRepository.Get(condition);
        }

        public bool SetProductStock(int productId, List<KeyValuePair<int, int>> sizeSku)
        {
            return false;
        }

        public IEnumerable<ProductStock> GetProductStockListByBaseIds(params int[] baseId)
        {
            var list = _productStockRepository.GetProductStockListByBaseIds(baseId);
            return list;
        }


        public int ResetProductStock(List<ProductStockExcelDTO> list)
        {
            if (list == null || !list.Any())
            {
                return 0;
            }

            var productList = _productRepository.GetProductListByIds(list.Select(t => t.ProductId).ToArray()).ToDictionary(c => c.ProductId, c => c);

            if (!productList.Any())
            {
                return 0;
            }

            var allSkus = SkuItemService.Instance.GetList(AgentEnums.SkuType.Size)
                .ToDictionary(c => c.SkuId, c => c.SkuName);

            var addList = new List<ProductStock>();
            foreach (var item in list)
            {
                if (!productList.ContainsKey(item.ProductId))
                {
                    continue;
                }
                var product = productList[item.ProductId];
                if (product.BaseId != item.BaseId || product.BaseNo != item.BaseNo.Trim() || product.ProductNo != item.ProductNo.Trim())
                {
                    continue;
                }
                if (!allSkus.ContainsKey(item.SkuId))
                {
                    continue;
                }
                if (allSkus[item.SkuId] != item.SkuName.Trim())
                {
                    continue;
                }
                addList.Add(new ProductStock()
                {
                    BaseId = item.BaseId,
                    ProductId = item.ProductId,
                    SkuId = item.SkuId,
                    SkuName = item.SkuName,
                    StockNum = item.StockNum,
                    UpdateDate = DateTime.Now
                });
            }
            var isSuccess = _productStockRepository.BatchInsert(addList);
            return isSuccess ? addList.Count : 0;
        }
    }
}
