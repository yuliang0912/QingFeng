using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.Models.DTO;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    public class GoodsController : CustomerController
    {
        // GET: Goods
        [AdminAuthorize(AgentEnums.SubMenuEnum.商品列表)]
        public ActionResult Index(string keyWord = "", int brandId = 1, int categoryId = 0, int page = 1,
            int pageSize = 30)
        {

            var totalItem = 0;
            var list = ProductService.Instance.SearchBaseProduct(brandId, 0, categoryId, keyWord, -1, page, pageSize,
                out totalItem);

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

        [AdminAuthorize(AgentEnums.SubMenuEnum.添加商品)]
        public ActionResult Add()
        {
            var sizeList = SkuItemService.Instance.GetList(AgentEnums.SkuType.Size).ToList();

            return View(sizeList);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.SubMenuEnum.编辑商品)]
        public ActionResult Edit(int baseId)
        {
            var baseInfo = ProductService.Instance.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }
            baseInfo.SubProduct = ProductService.Instance.GetProductByBaseId(baseId);

            var skuDict = ProductService.Instance.GetProductSkuListByBaseId(baseId)
                .GroupBy(t => t.ProductId)
                .ToDictionary(c => c.Key, c => c.ToList());

            foreach (var product in baseInfo.SubProduct)
            {
                if (skuDict.ContainsKey(product.ProductId))
                {
                    product.ProductSkus = skuDict[product.ProductId];
                }
            }

            ViewBag.sizeList = SkuItemService.Instance.GetList(AgentEnums.SkuType.Size).ToList();

            return View(baseInfo);
        }

        /// <summary>
        /// 商品上下架
        /// </summary>
        /// <param name="baseId"></param>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.SubMenuEnum.上下架商品)]
        public ActionResult Set(int baseId)
        {
            var baseInfo = ProductService.Instance.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }

            baseInfo.SubProduct = ProductService.Instance.GetProductByBaseId(baseId);

            return View(baseInfo);
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="baseId"></param>
        /// <returns></returns>
        [AdminAuthorize(AgentEnums.SubMenuEnum.查看商品)]
        public ActionResult View(int baseId)
        {
            var baseInfo = ProductService.Instance.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }
            baseInfo.SubProduct = ProductService.Instance.GetProductByBaseId(baseId);

            ViewBag.productSkus =
                ProductService.Instance.GetProductSkuListByBaseId(baseId).ToList()
                    .GroupBy(t => t.SkuName).Select(t => t.Key).ToList();

            return View(baseInfo);
        }

        #region Ajax

        /// <summary>
        /// 保存创建商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize(AgentEnums.SubMenuEnum.添加商品)]
        public JsonResult Add(CreateProductDto model)
        {
            var result = ProductService.Instance.AddProduct(model, new UserInfo() { UserId = 155014 });

            return Json(result);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.上下架商品)]
        public JsonResult Set()
        {
            var baseId = Convert.ToInt32(Request.Form["baseId"] ?? string.Empty);

            var productList = ProductService.Instance.GetProductByBaseId(baseId);

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

            var result = ProductService.Instance.UpdateStatus(baseId, baseStatus, list);

            return Json(result);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑商品)]
        public JsonResult Edit(CreateProductDto model)
        {
            var baseInfo = ProductService.Instance.GetProductBase(model.baseId);
            if (baseInfo == null)
            {
                return Json(new ApiResult<bool>(false)
                {
                    ErrorCode = 1,
                    Message = "数据错误,未找到产品!"
                });
            }

            baseInfo.BaseName = model.baseName.Trim();
            baseInfo.BaseNo = model.baseNo.Trim();
            baseInfo.BrandId = model.brandId;
            baseInfo.CategoryId = model.categoryId;
            baseInfo.SexId = model.sex;

            var existsProduct = baseInfo.SubProduct.ToDictionary(c => c.ProductNo, c => c);

            var list = new List<Product>();
            foreach (var item in model.subProduct)
            {
                var subProduct = new Product()
                {
                    BaseId = baseInfo.BaseId,
                    BaseNo = baseInfo.BaseNo,
                    BaseName = baseInfo.BaseName,
                    ProductName = baseInfo.BaseName,
                    ProductNo = item.color.Trim(),
                    OriginalPrice = item.sizeList.First().sizePrice,
                    ActualPrice = item.lowestPrice,
                    CreateDate = DateTime.Now,
                    Status = 0,
                };
                if (existsProduct.ContainsKey(subProduct.ProductNo))
                {
                    subProduct.ProductId = existsProduct[subProduct.ProductNo].ProductId;
                    subProduct.CreateDate = existsProduct[subProduct.ProductNo].CreateDate;
                }
                subProduct.ProductSkus = item.sizeList.Select(x => new ProductSkus()
                {
                    BaseId = baseInfo.BaseId,
                    ProductId = subProduct.ProductId,
                    SkuId = x.sizeId,
                    SkuName = x.sizeName,
                    Price = x.sizePrice,
                    Status = 0,
                    UpdateDate = DateTime.Now
                }).ToList();

                list.Add(subProduct);
            }

            baseInfo.SubProduct = list;
            var result = ProductService.Instance.EditBaseProduct(baseInfo);

            return Json(result);
        }

        [AdminAuthorize]
        public JsonResult SearchProduct(string keyWords)
        {
            var list = ProductService.Instance.SearchProduct(keyWords);

            var baseList = ProductService.Instance.GetProductBaseList(list.Select(t => t.BaseId).ToArray())
                .ToDictionary(c => c.BaseId, c => c);

            return Json(list.Select(x => new
            {
                baseId = x.BaseId,
                baseNo = baseList[x.BaseId].BaseNo,
                productId = x.ProductId,
                baseName = x.BaseName,
                productName = x.ProductName,
                productNo = x.ProductNo,
                originalPrice = x.OriginalPrice,
                actualPrice = x.ActualPrice,
                categoryName = baseList[x.BaseId].CategoryId.ToString()
            }));
        }
        #endregion
    }
}