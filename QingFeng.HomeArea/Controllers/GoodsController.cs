using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Business.V2;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.Models.DTO;
using ProductService = QingFeng.Business.ProductService;

namespace QingFeng.WebArea.Controllers
{
    public class GoodsController : CustomerController
    {
        private readonly SkuItemService _skuItemService = new SkuItemService();
        private readonly ProductService _productService = new ProductService();


        // GET: Goods
        public ActionResult Index(string keyWord = "", int brandId = 1, int categoryId = 0, int page = 1,
            int pageSize = 30)
        {
            var totalItem = 0;
            var list = _productService.SearchBaseProduct(keyWord, categoryId, -1, page, pageSize, out totalItem);

            ViewBag.brandId = brandId;
            ViewBag.categoryId = categoryId;
            ViewBag.keyWord = keyWord;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }


        public ActionResult Add()
        {
            var sizeList = _skuItemService.GetList(AgentEnums.SkuType.Size).ToList();

            return View(sizeList);
        }

        /// <summary>
        /// 保存创建商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(CreateProductDto model)
        {
            var result = _productService.AddProduct(model, new UserInfo() {UserId = 155014});

            return Json(result);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int baseId)
        {
            var baseInfo = _productService.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }
            baseInfo.SubProduct = _productService.GetProductByBaseId(baseId);

            var skuDict = _productService.GetProductSkuListByBaseId(baseId)
                .GroupBy(t => t.ProductId)
                .ToDictionary(c => c.Key, c => c.ToList());

            foreach (var product in baseInfo.SubProduct)
            {
                if (skuDict.ContainsKey(product.ProductId))
                {
                    product.ProductSkus = skuDict[product.ProductId];
                }
            }

            ViewBag.sizeList = _skuItemService.GetList(AgentEnums.SkuType.Size).ToList();

            return View(baseInfo);
        }

        /// <summary>
        /// 商品上下架
        /// </summary>
        /// <param name="baseId"></param>
        /// <returns></returns>
        public ActionResult Set(int baseId)
        {
            var baseInfo = _productService.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }

            baseInfo.SubProduct = _productService.GetProductByBaseId(baseId);

            return View(baseInfo);
        }

        [HttpPost]
        public JsonResult Set()
        {
            var baseId = Convert.ToInt32(Request.Form["baseId"] ?? string.Empty);

            var productList = _productService.GetProductByBaseId(baseId);

            var list = new List<KeyValuePair<int, int>>();
            foreach (var item in productList)
            {
                var status = Request.Form[$"spu[{item.ProductId}]"];
                if (!string.IsNullOrEmpty(status))
                {
                    item.Status = Convert.ToInt32(status);
                    list.Add(new KeyValuePair<int, int>(item.ProductId, item.Status));
                }
            }

            var baseStatus = productList.Any(t => t.Status == 0) ? 0 : 1;

            var result = _productService.UpdateStatus(baseId, baseStatus, list);

            return Json(result);
        }


        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="baseId"></param>
        /// <returns></returns>
        public ActionResult View(int baseId)
        {
            var baseInfo = _productService.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }
            baseInfo.SubProduct = _productService.GetProductByBaseId(baseId);

            ViewBag.productSkus =
                _productService.GetProductSkuListByBaseId(baseId).ToList()
                    .GroupBy(t => t.SkuName).Select(t => t.Key).ToList();

            return View(baseInfo);
        }
    }
}