using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    public class ProductController : CustomerController
    {
        private readonly ProductService _productService = new ProductService();
        private readonly ProductStockService _productStockService = new ProductStockService();

        /// <summary>
        /// 添加基础产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult AddBaseProduct(ProductBase model)
        {
            if (model == null)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据接收失败"});
            }

            var result = _productService.CreateBaseProduct(model);

            return Json(new ApiResult<bool>(result));
        }

        /// <summary>
        /// 根据颜色自动创建子商品
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="colorSku"></param>
        /// <returns></returns>
        [HttpPost, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult CreateProduct(int baseId, List<KeyValuePair<int, string>> colorSku)
        {
            var baseProduct = _productService.GetProductBase(baseId);

            if (null == baseProduct)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "未找到商品"});
            }
            if (colorSku == null || colorSku.Count < 1)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "颜色SKU必须指定一个"});
            }
            var result = _productService.CreateProduct(baseId, colorSku);

            return Json(new ApiResult<int>(result));
        }

        /// <summary>
        /// 获取产品库存
        /// </summary>
        /// <param name="baseNo"></param>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.UserRole.AllUser)]
        public JsonResult GetProductStock(string baseNo)
        {
            var model = _productService.GetProductBase(baseNo);

            if (model == null)
            {
                return Json(Enumerable.Empty<object>());
            }

            model.SubProduct = _productService.GetProductByBaseId(model.BaseId);

            var productStockList = _productStockService.GetList(new {model.BaseId});

            var productStocks = productStockList
                .GroupBy(t => t.ProductId)
                .ToDictionary(t => t.Key, t => t);

            model.SubProduct.ToList().ForEach(t =>
            {
                if (productStocks.ContainsKey(t.ProductId))
                {
                    t.ProductStocks = productStocks[t.ProductId].ToList();
                }
            });

            var jsonData = model.SubProduct.Select(x => new
            {
                baseId = model.BaseId,
                baseName = model.BaseName,
                productId = x.ProductId,
                productName = x.ProductName,
                productNo = x.ProductNo,
                Category = model.CategoryId.ToString(),
                skuList = productStockList.GroupBy(t => t.SkuId).Select(m => new
                {
                    skuId = m.Key,
                    skuName = m.First().SkuName
                }),
                productStocks = x.ProductStocks
            });

            return Json(jsonData);
        }

        //搜索产品库
        public JsonResult SearchProduct(int categoryId, string keyWords, int page, int pageSize)
        {
            int totalItem;

            var list = _productService.SearchBaseProduct(keyWords, categoryId, page, pageSize, out totalItem);

            return Json(new ApiPageList<ProductBase>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }
    }
}