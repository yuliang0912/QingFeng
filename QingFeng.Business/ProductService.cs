using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using System.Transactions;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models.DTO;

namespace QingFeng.Business
{

    public class ProductService : Singleton<ProductService>
    {

        private ProductService() { }
       
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductSkuRepository _productSkuRepository = new ProductSkuRepository();
        private readonly ProductBaseRepository _productBaseRepository = new ProductBaseRepository();
        private readonly SkuItemRepository _skuItemRepository = new SkuItemRepository();

        public ApiResult<bool> AddProduct(CreateProductDto model, UserInfo user)
        {
            if (_productBaseRepository.Count(new { baseNo = model.baseNo.Trim() }) > 0)
            {
                return new ApiResult<bool>(false) { ErrorCode = 1, Message = "当前商品已经存在,不能重复添加" };
            }

            var baseProduct = new ProductBase
            {
                BaseName = model.baseName.Trim(),
                BaseNo = model.baseNo.Trim(),
                CategoryId = model.categoryId,
                BrandId = model.brandId,
                SexId = model.sex,
                Status = 0,
                CreateUserId = user.UserId,
                CreateDate = DateTime.Now,
                SubProduct = model.subProduct.Select(item =>
                {
                    var product = new Product
                    {
                        ProductName = model.baseName,
                        ProductNo = item.color,
                        BaseNo = model.baseNo,
                        BaseName = model.baseName,
                        OriginalPrice = item.sizeList.First().sizePrice,
                        ActualPrice = item.lowestPrice,
                        Status = 0,
                        CreateDate = DateTime.Now,
                        ProductSkus = item.sizeList.Select(size => new ProductSkus()
                        {
                            SkuId = size.sizeId,
                            SkuName = size.sizeName,
                            Price = size.sizePrice,
                            Status = 0,
                            UpdateDate = DateTime.Now
                        })
                    };

                    return product;
                })
            };


            var result = _productBaseRepository.CreateProduct(baseProduct);

            return new ApiResult<bool>(result);
        }

        public int CreateProduct(Product model)
        {
            if (string.IsNullOrWhiteSpace(model.ProductName) || string.IsNullOrWhiteSpace(model.ProductNo))
            {
                return 2;
            }
            var baseInfo = _productBaseRepository.Get(new { model.BaseId });
            if (baseInfo == null)
            {
                return 0;
            }
            if (_productRepository.Count(new { model.ProductNo }) > 0)
            {
                return 3;
            }

            model.BaseNo = baseInfo.BaseNo;
            model.BaseName = baseInfo.BaseName;
            model.CreateDate = DateTime.Now;

            return _productRepository.Insert(model) > 0 ? 1 : 0;
        }

        public int EditProduct(Product model)
        {
            var originalInfo = _productRepository.Get(new { model.ProductId });
            if (originalInfo == null)
            {
                return 2;
            }
            if (string.IsNullOrWhiteSpace(model.ProductName) || string.IsNullOrWhiteSpace(model.ProductNo))
            {
                return 3;
            }
            if (!model.ProductNo.Equals(originalInfo.ProductNo) && _productRepository.Count(new { model.ProductNo }) > 0)
            {
                return 4;
            }
            if (model.OriginalPrice < 0 || model.ActualPrice < 0)
            {
                return 6;
            }

            return _productRepository.Update(
                new { model.ProductNo, model.ProductName, model.OriginalPrice, model.ActualPrice },
                new { model.ProductId })
                ? 1
                : 0;
        }

        public bool UpdateProductBaseInfo(object model, object condition)
        {
            return _productBaseRepository.Update(model, condition);
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
            var model = _productBaseRepository.Get(new {baseId});

            if (model != null)
            {
                model.SubProduct = _productRepository.GetProductListByBaseIds(0, model.BaseId);
            }
            return model;
        }

        public IEnumerable<ProductBase> GetProductBaseList(params int[] baseId)
        {
            return _productBaseRepository.GetListByIds(baseId);
        }

        public ProductBase GetProductBase(string baseNo)
        {
            var model = _productBaseRepository.Get(new {baseNo});

            if (model != null)
            {
                model.SubProduct = _productRepository.GetProductListByBaseIds(0, model.BaseId);
            }
            return model;
        }

        public IEnumerable<Product> GetProduct(params int[] productId)
        {
            return _productRepository.GetProductListByIds(productId);
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.Get(new { productId });
        }

        public IEnumerable<Product> GetProductByBaseId(int baseId)
        {
            return _productRepository.GetList(new { baseId });
        }

        public IEnumerable<ProductBase> GetBaseProductList(object condition)
        {
            var list = _productBaseRepository.GetList(condition);
            if (list.Any())
            {
                var productDict = _productRepository.GetProductListByBaseIds(-1, list.Select(t => t.BaseId).ToArray())
                    .GroupBy(c => c.BaseId)
                    .ToDictionary(c => c.Key, c => c);

                list.ToList().ForEach(t =>
                {
                    if (productDict.ContainsKey(t.BaseId))
                    {
                        t.SubProduct = productDict[t.BaseId];
                    }
                });
            }
            return list;
        }

        public bool UpdateStatus(int baseId, int baseStatus, List<KeyValuePair<int, int>> productStatus)
        {
            using (var trans = new TransactionScope())
            {
                _productBaseRepository.Update(new { status = baseStatus }, new { baseId });
                foreach (var item in productStatus)
                {
                    _productRepository.Update(new { status = item.Value }, new { productId = item.Key });
                }
                trans.Complete();
            }
            return true;
        }

        public IEnumerable<ProductBase> SearchBaseProduct(int brandId, int sexId, int categoryId, string keyWords, int status, int page, int pageSize,
            out int totalItem)
        {
            var list = _productBaseRepository.SearchProductBase(brandId, sexId, categoryId, keyWords, status, page, pageSize,
                out totalItem);

            if (!list.Any()) return list;

            var productDict = _productRepository.GetProductListByBaseIds(status, list.Select(t => t.BaseId).ToArray())
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

        public IEnumerable<ProductBase> SearchBaseProduct(int categoryId, int status, string keyWords)
        {
            int totalItem;
            return _productBaseRepository.SearchProductBase(0, 0, categoryId, keyWords, status, 1, 100, out totalItem);
        }

        public IEnumerable<Product> SearchProduct(string keyWords)
        {
            int totalItem;
            return _productRepository.SearchProduct(keyWords, 1, 150, out totalItem);
        }

        public IEnumerable<ProductSkus> GetProductSkuListByBaseId(int baseId)
        {
            return _productSkuRepository.GetList(new { baseId });
        }

        public IEnumerable<ProductSkus> GetProductSkuListByBaseIds(params int[] baseId)
        {
            return _productSkuRepository.GetProductSkuListByBaseIds(baseId);
        }

        public List<KeyValuePair<int, string>> GetDistinctSkuList(params int[] baseId)
        {
            return _productSkuRepository.GetDistinctSkuList(baseId);
        }
    }
}
