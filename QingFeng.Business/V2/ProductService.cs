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
            if (_productBaseRepository.Count(new { baseNo = model.baseNo.Trim() }) > 0)
            {
                return new ApiResult<bool>(false) { ErrorCode = 1, Message = "当前商品已经存在,不能重复添加" };
            }

            var baseProduct = new ProductBase()
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

            baseProduct.SubProduct = model.subProduct.Select(item =>
             {
                 var product = new Product()
                 {
                     ProductName = model.baseName,
                     ProductNo = item.color,
                     BaseNo = model.baseNo,
                     BaseName = model.baseName,
                     OriginalPrice = item.sizeList.First().sizePrice,
                     ActualPrice = item.lowestPrice,
                     Status = 0,
                     CreateDate = DateTime.Now
                 };

                 product.ProductSkus = item.sizeList.Select(size =>
                 {
                     return new ProductSkus()
                     {
                         SkuId = size.sizeId,
                         Price = size.sizePrice,
                         Status = 0,
                         UpdateDate = DateTime.Now
                     };
                 });
                 return product;
             });

            var result = _productBaseRepository.CreateProduct(baseProduct);

            return new ApiResult<bool>(result);
        }
    }
}
