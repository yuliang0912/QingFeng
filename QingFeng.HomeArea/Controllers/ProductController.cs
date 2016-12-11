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

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public ActionResult EditProduct(int productId)
        {
            var model = _productService.GetProduct(productId);

            if (model == null)
            {
                return Content("未找到有效产品");
            }

            var skuList = _skuItemService.GetList(AgentEnums.SkuType.Color);

            ViewBag.ColorSku = skuList.Where(t => t.Status == 0)
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                .ToList();

            return View(model);
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public ActionResult SubProducts(int baseId)
        {
            var list = _productService.GetProductByBaseId(baseId);
            return View(list);
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public ActionResult ProductManger(int baseId)
        {
            var baseProduct = _productService.GetProductBase(baseId);

            if (baseProduct == null)
            {
                return Content("未找到有效产品");
            }

            baseProduct.SubProduct = _productService.GetProductByBaseId(baseId).ToList();

            var skuIds = baseProduct.SubProduct.Select(t => t.ColorId).ToList();

            var skuList = _skuItemService.GetList(AgentEnums.SkuType.Color);

            ViewBag.ColorSku = skuList.Where(t => t.Status == 0 && !skuIds.Contains(t.SkuId))
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                .ToList();

            return View(baseProduct);
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public ActionResult Products(string keyWords = "", int categoryId = 0, int page = 1, int pageSize = 20)
        {
            int totalItem;

            var list = _productService.SearchBaseProduct(keyWords, categoryId, 0, page, pageSize, out totalItem);

            ViewBag.categoryId = categoryId;
            ViewBag.keyWords = keyWords;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
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

            return View(baseProduct);
        }

        #region Ajax

        [HttpPost, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult AddProduct(Product model)
        {
            return Json(_productService.CreateProduct(model));
        }


        [HttpPost, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult EditProduct(Product model)
        {
            return Json(_productService.EditProduct(model));
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult UpdateBaserProductStatus(int baseId, int status)
        {
            var result = _productService.UpdateBaseProductInfo(new {status}, new {baseId});
            if (result && status == 1)
            {
                _productService.UpdateProductInfo(new {status}, new {baseId});
            }
            return Json(result);
        }

        [HttpGet, AdminAuthorize(AgentEnums.UserRole.Administrator)]
        public JsonResult UpdateProductStatus(int productId, int status)
        {
            return Json(_productService.UpdateProductInfo(new {status}, new {productId}));
        }

        /// <summary>
        /// 根据颜色自动创建子商品
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="colorSkuIds"></param>
        /// <returns></returns>
        [HttpPost, AdminAuthorize(AgentEnums.UserRole.Administrator)]
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
        [AdminAuthorize(AgentEnums.UserRole.StoreUser)]
        public JsonResult SearchProduct(int categoryId, string keyWords, int page, int pageSize)
        {
            int totalItem;

            var list = _productService.SearchBaseProduct(keyWords, categoryId, 0, page, pageSize, out totalItem);

            return Json(new ApiPageList<ProductBase>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        #endregion
    }
}