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
        [HttpGet, AdminAuthorize]
        public ActionResult EditProduct(int productId)
        {
            var model = ProductService.Instance.GetProduct(productId);

            if (model == null)
            {
                return Content("未找到有效产品");
            }

            var skuList = SkuItemService.Instance.GetList(AgentEnums.SkuType.Color);

            ViewBag.ColorSku = skuList.Where(t => t.Status == 0)
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                .ToList();

            return View(model);
        }

        [HttpGet, AdminAuthorize]
        public ActionResult SubProducts(int baseId)
        {
            var list = ProductService.Instance.GetProductByBaseId(baseId);
            return View(list);
        }

        [HttpGet, AdminAuthorize]
        public ActionResult ProductManger(int baseId)
        {
            var baseProduct = ProductService.Instance.GetProductBase(baseId);

            //if (baseProduct == null)
            //{
            //    return Content("未找到有效产品");
            //}

            //baseProduct.SubProduct = ProductService.Instance.GetProductByBaseId(baseId).ToList();

            //var skuIds = baseProduct.SubProduct.Select(t => t.ColorId).ToList();

            //var skuList = SkuItemService.Instance.GetList(AgentEnums.SkuType.Color);

            //ViewBag.ColorSku = skuList.Where(t => t.Status == 0 && !skuIds.Contains(t.SkuId))
            //    .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
            //    .ToList();

            return View(baseProduct);
        }

        [HttpGet, AdminAuthorize]
        public ActionResult Products(string keyWords = "", int categoryId = 0, int page = 1, int pageSize = 20)
        {
            int totalItem;

            var list = ProductService.Instance.SearchBaseProduct(0, 0, categoryId, keyWords, 0, page, pageSize, out totalItem);

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

        [HttpGet, AdminAuthorize]
        public ActionResult CreateProduct(int baseId)
        {
            var baseProduct = ProductService.Instance.GetProductBase(baseId);

            if (baseProduct == null)
            {
                return Content("未找到有效产品");
            }

            var skuList = SkuItemService.Instance.GetList(AgentEnums.SkuType.Color);

            ViewBag.ColorSku = skuList.Where(t => t.Status == 0)
                .Select(t => new KeyValuePair<int, string>(t.SkuId, t.SkuName))
                .ToList();

            return View(baseProduct);
        }

        #region Ajax

        [HttpPost, AdminAuthorize]
        public JsonResult AddProduct(Product model)
        {
            return Json(ProductService.Instance.CreateProduct(model));
        }


        [HttpPost, AdminAuthorize]
        public JsonResult EditProduct(Product model)
        {
            return Json(ProductService.Instance.EditProduct(model));
        }

        [HttpGet, AdminAuthorize]
        public JsonResult UpdateBaserProductStatus(int baseId, int status)
        {
            var result = ProductService.Instance.UpdateBaseProductInfo(new {status}, new {baseId});
            if (result && status == 1)
            {
                ProductService.Instance.UpdateProductInfo(new {status}, new {baseId});
            }
            return Json(result);
        }

        [HttpGet, AdminAuthorize]
        public JsonResult UpdateProductStatus(int productId, int status)
        {
            return Json(ProductService.Instance.UpdateProductInfo(new {status}, new {productId}));
        }

        /// <summary>
        /// 获取产品库存
        /// </summary>
        /// <param name="baseNo"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public JsonResult GetProductStock(string baseNo)
        {
            var model = ProductService.Instance.GetProductBase(baseNo);

            if (model == null)
            {
                return Json(Enumerable.Empty<object>());
            }

            model.SubProduct = ProductService.Instance.GetProductByBaseId(model.BaseId);

            var productStockList = ProductStockService.Instance.GetList(new {model.BaseId});

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
        [AdminAuthorize]
        public JsonResult SearchProduct(int categoryId, string keyWords, int page, int pageSize)
        {
            int totalItem;

            var list = ProductService.Instance.SearchBaseProduct(0, 0, categoryId, keyWords, 0, page, pageSize, out totalItem);

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