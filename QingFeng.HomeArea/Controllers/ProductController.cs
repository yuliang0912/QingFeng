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
        private readonly SkuItemService _skuItemService = new SkuItemService();


        public ActionResult EditProduct(int productId)
        {
            var model = _productService.GetProduct(productId);

            if (model == null)
            {
                return Content("未找到有效产品");
            }

            return View(model);
        }

        public ActionResult SubProducts(int baseId)
        {
            var list = _productService.GetProductByBaseId(baseId);
            return View(list);
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.AllUser)]
        public ActionResult CreateProduct(int baseId)
        {
            var baseProduct = _productService.GetProductBase(baseId);

            if (baseProduct == null)
            {
                return Content("未找到有效产品");
            }

            var skuList = _skuItemService.GetList(AgentEnums.SkuType.Color);

            ViewBag.ColorSku = skuList.Where(t => t.Status == 0)
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                .ToList();

            ViewBag.SubProducts = _productService.GetProductByBaseId(baseId).ToList();

            return View(baseProduct);
        }

        /// <summary>
        /// 根据颜色自动创建子商品
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="colorSkuIds"></param>
        /// <returns></returns>
        [HttpPost, AdminAuthorize(AgentEnums.UserRole.AllUser)]
        public JsonResult CreateProduct(int baseId, string colorSkuIds)
        {
            var baseProduct = _productService.GetProductBase(baseId);

            if (null == baseProduct)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "未找到商品"});
            }
            if (string.IsNullOrWhiteSpace(colorSkuIds))
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "颜色SKU必须指定一个"});
            }
            var colorList = colorSkuIds.Split(',').Select(int.Parse).ToList();
            var result = _productService.CreateProduct(baseId, colorList);

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
                skuList = productStockList.Where(t => t.StockNum > 0).GroupBy(t => t.SkuId).Select(m => new
                {
                    skuId = m.Key,
                    skuName = m.First().SkuName
                }),
                productStocks = x.ProductStocks
            });

            return Json(jsonData);
        }

        //搜索产品库
        [AdminAuthorize(AgentEnums.UserRole.AllUser)]
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