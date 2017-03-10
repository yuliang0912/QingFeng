using QingFeng.Business;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    public class StockController : Controller
    {

        public ActionResult Search(string keyWords = "", int brandId = 0, int sexId = 0, int warehouseId = 0, int page = 1, int pageSize = 20)
        {
            int totalItem;

            var list = ProductService.Instance.SearchBaseProduct(brandId, sexId, 0, keyWords, 0, page, pageSize, out totalItem);

            var productStocks = ProductStockService.Instance.GetProductStockListByBaseIds(list.Select(t => t.BaseId).ToArray())
                .GroupBy(t => t.ProductId)
                .ToDictionary(c => c.Key, c => c.ToList());

            foreach (var item in list.SelectMany(x => x.SubProduct))
            {
                if (productStocks.ContainsKey(item.ProductId))
                {
                    item.ProductStocks = productStocks[item.ProductId];
                    item.StockNum = productStocks[item.ProductId].Sum(t => t.StockNum);
                }
            }

            ViewBag.allSkus = productStocks.SelectMany(t => t.Value)
                .GroupBy(t => t.SkuId)
                .ToDictionary(t => t.Key, t => t.First().SkuName);


            ViewBag.brandId = brandId;
            ViewBag.sexId = sexId;
            ViewBag.keyWords = keyWords;
            ViewBag.warehouseId = warehouseId;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }
    }
}