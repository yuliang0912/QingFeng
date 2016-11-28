using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Common.Extensions;
using System.Dynamic;
using QingFeng.Common;

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
            foreach (var sku in colorSku.OrderBy(t => t.Key))
            {
                if (!productList.Exists(t => t.ColorId == sku.Key))
                {
                    var product = new Product()
                    {
                        BaseId = baseInfo.BaseId,
                        BaseName = baseInfo.BaseName,
                        OriginalPrice = baseInfo.OriginalPrice,
                        ActualPrice = baseInfo.ActualPrice,
                        ImgList = baseInfo.ImgList,
                        ColorId = sku.Key,
                        ProductName = baseInfo.BaseName + "-" + sku.Value,
                        ProductNo = baseInfo.BaseNo + "-" + StringExtensions.FillZeroNumber(sku.Key, 3),
                        CreateDate = DateTime.Now
                    };

                    if (_productRepository.Insert(product, true) > 0)
                    {
                        addCount++;
                    }
                    System.Threading.Thread.Sleep(50);
                }
                else
                {
                    _productRepository.Update(new {status = 0}, new {baseId, colorId = sku.Key});
                }
            }
            return addCount;
        }

        public bool CreateBaseProduct(ProductBase model)
        {
            model.Intro = (model.Intro ?? string.Empty).CutString(600);
            model.ImgList = model.ImgList ?? string.Empty;
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

        public IEnumerable<ProductBase> GetProductBaseList(params int[] baseId)
        {
            return _productBaseRepository.GetListByIds(baseId);
        }

        public ProductBase GetProductBase(string baseNo)
        {
            return _productBaseRepository.Get(new {baseNo});
        }

        public IEnumerable<Product> GetProduct(params int[] productId)
        {
            return _productRepository.GetProductListByIds(productId);
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
            var list = _productBaseRepository.SearchProductBase(categoryId, keyWords, page, pageSize, out totalItem);

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
