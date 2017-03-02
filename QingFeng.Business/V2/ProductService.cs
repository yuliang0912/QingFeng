using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using QingFeng.Common.ApiCore.Result;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Models.DTO;

namespace QingFeng.Business.V2
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new ProductRepository();
        private readonly ProductBaseRepository _productBaseRepository = new ProductBaseRepository();
        private readonly ProductStockRepository _productStockRepository = new ProductStockRepository();


        public ApiResult<bool> AddProduct(CreateProductDto model, UserInfo user)
        {
            var subProductList = _productRepository.GetList(new {model.baseNo}).ToList();

            if (subProductList.Any(t => model.subProduct.Select(m => m.color.Trim()).Contains(t.ProductNo)))
            {
                return new ApiResult<bool>(false) {ErrorCode = 1, Message = "当前商品已经存在,不能重复添加"};
            }

            model.baseNo = model.baseNo.Trim();

            var baseProduct = _productBaseRepository.Get(new {model.baseNo});

            using (var trans = new TransactionScope())
            {
                if (baseProduct == null)
                {
                    baseProduct = new ProductBase()
                    {
                        BaseName = model.baseName.Trim(),
                        BaseNo = model.baseNo.Trim(),
                        CategoryId = model.categoryId,
                        BrandId = model.brandId,
                        SexId = model.sex,
                        Status = 0,
                        CreateUserId = user.UserId,
                        CreateDate = DateTime.Now
                    };
                    baseProduct.BaseId = _productBaseRepository.Insert(baseProduct, true);
                }
                else
                {
                    _productBaseRepository.Update(new
                    {
                        CategoryId = model.categoryId,
                        BrandId = model.brandId,
                        SexId = model.sex,
                    }, new {baseProduct.BrandId});
                }

                var productStockList = new List<ProductStock>();
                foreach (var item in model.subProduct)
                {
                    int productId = _productRepository.Insert(new Product()
                    {
                        ProductName = model.baseName,
                        ProductNo = item.color,
                        BaseId = baseProduct.BaseId,
                        BaseNo = model.baseNo,
                        BaseName = model.baseName,
                        OriginalPrice = item.sizeList.First().sizePrice,
                        ActualPrice = item.lowestPrice,
                        Status = 0,
                        CreateDate = DateTime.Now
                    }, true);

                    foreach (var size in item.sizeList)
                    {
                        productStockList.Add(new ProductStock()
                        {
                            BaseId = baseProduct.BaseId,
                            ProductId = productId,
                            SkuId = size.sizeId,
                            SkuName = size.sizeName,
                            StockNum = 0,
                            UpdateDate = DateTime.Now
                        });
                    }
                }
                _productStockRepository.BatchInsert(productStockList);
                trans.Complete();
            }

            return new ApiResult<bool>(true);
        }
    }
}
