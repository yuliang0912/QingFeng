using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Common.Extensions;
using System.Dynamic;

namespace QingFeng.Business
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductBaseRepository _productBaseRepository = new ProductBaseRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();


        public int CreateProduct(int baseId, List<KeyValuePair<int, string>> colorSku)
        {
            var baseInfo = _productBaseRepository.Get(new {baseId});

            if (baseInfo == null)
            {
                return 0;
            }

            var productList = _productRepository.GetList(new {baseId}).ToList();

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
            return _productBaseRepository.Get(new {baseId});
        }

        public ProductBase GetProductBase(string baseNo)
        {
            return _productBaseRepository.Get(new {baseNo});
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.Get(new {productId});
        }

        public IEnumerable<Product> GetProductByBaseId(int baseId)
        {
            return _productRepository.GetList(new {baseId});
        }

        public IEnumerable<ProductBase> SearchBaseProduct(string keyWords, int categoryId, int page, int pageSize,
            out int totalItem)
        {
            dynamic condition = new ExpandoObject();

            condition.keyWords = keyWords.FormatSqlLikeString();

            if (categoryId > 0)
            {
                condition.categoryId = categoryId;
            }

            var list = _productBaseRepository.SearchProductBase((object) condition, page, pageSize, out totalItem);

            if (!list.Any()) return list;

            var productDict = _productRepository.GetProductListByBaseIds(list.Select(t => t.BaseId).ToArray())
                .GroupBy(c => c.BaseId)
                .ToDictionary(c => c.Key, c => c);

            list.ToList().ForEach(t =>
            {
                if (productDict.ContainsKey(t.BaseId))
                {
                    t.SubProduct = productDict[t.BaseId];
                }
            });

            return list;
        }


        public IEnumerable<Product> SearchProduct(string keyWords)
        {
            int totalItem;
            return _productRepository.SearchProduct(keyWords, 1, 150, out totalItem);
        }
    }
}
