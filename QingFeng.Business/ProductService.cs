using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Common.Extensions;

namespace QingFeng.Business
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductBaseRepository _productBaseRepository = new ProductBaseRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();


        public int CreateProduct(int baseId, List<KeyValuePair<int, string>> colorSku)
        {
            var baseInfo = _productBaseRepository.Get(new { baseId });

            if (baseInfo == null)
            {
                return 0;
            }

            var productList = _productRepository.GetList(new { baseId }).ToList();

            var addCount = 0;
            foreach (var sku in colorSku)
            {
                if (!productList.Exists(t => t.ColorId == sku.Key))
                {
                    var product = new Product()
                    {
                        ProductName = baseInfo.BaseName + " " + sku.Value,
                        BaseId = baseInfo.BaseId,
                        BaseName = baseInfo.BaseName,
                        OriginalPrice = baseInfo.OriginalPrice,
                        ActualPrice = baseInfo.ActualPrice,
                        ProductNo = baseInfo.BaseNo + StringExtensions.FillZeroNumber(sku.Key, 3)
                    };

                    if (_productRepository.Insert(product, true) > 0)
                    {
                        addCount++;
                    }
                    System.Threading.Thread.Sleep(50);
                }
            }
            return addCount;
        }

        public bool CreateBaseProduct(ProductBase model)
        {
            model.CreateDate = DateTime.Now;
            return _productBaseRepository.Insert(model) > 0;
        }

        public bool UpdateProductInfo(object model, object condition)
        {
            return _productRepository.Update(model, condition);
        }

        public bool UpdateBaseProductInfo(object model, object condition)
        {
            return _productBaseRepository.Update(model, condition);
        }

        public ProductBase GetProductBase(int baseId)
        {
            return _productBaseRepository.Get(new { baseId });
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.Get(new { productId });
        }

        public IEnumerable<ProductBase> SearchProduct(string keyWords, int page, int pageSize, out int totalItem)
        {
            var list = _productBaseRepository.SearchProductBase(new { keyWords }, page, pageSize, out totalItem);

            return list;
        }
    }
}
