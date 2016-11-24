using System;
using System.Collections.Generic;
using System.Linq;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;

namespace QingFeng.TestConsole
{
    public static class ProductUnitTest
    {
        private static readonly ProductService ProductService = new ProductService();
        private static readonly ProductStockService ProductStockService = new ProductStockService();

        public static void CreateBaseProductTest()
        {
            var result = ProductService.CreateBaseProduct(new ProductBase()
            {
                BaseName = "女士休闲鞋",
                OriginalPrice = 999.9m,
                ActualPrice = 450,
                BaseNo = "7-29SPW1030",
                CategoryId = AgentEnums.Category.女鞋,
                CreateUserId = 10006
            });
            Console.WriteLine("创建基础产品测试结果:{0}", result);
        }

        public static void CreateProductTest()
        {
            var baseId = 2;

            var colorSku = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "红色"),
                new KeyValuePair<int, string>(3, "蓝色")
            };

            var result = ProductService.CreateProduct(baseId, colorSku);
            Console.WriteLine("创建产品测试结果:{0}", result);
        }

        public static void SearchProductTest()
        {
            int totalItem;

            var keyWords = "0035";
            var categoryId = 1;

            var list = ProductService.SearchBaseProduct(keyWords, categoryId, 1, 10, out totalItem);

            var result = new ApiPageList<ProductBase>()
            {
                Page = 1,
                PageSize = 10,
                TotalCount = totalItem,
                PageList = list
            };
            Console.WriteLine("搜索产品测试结果:");
            Console.WriteLine(JsonHelper.Encode(result));
        }

        public static void SearchProductStockTest()
        {
            var baseNo = "7-29SPW1030";

            var model = ProductService.GetProductBase(baseNo);


            model.SubProduct = ProductService.GetProductByBaseId(model.BaseId);

            var productStockList = ProductStockService.GetList(new {model.BaseId});

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

            var jsonData = new
            {
                skuList = productStockList.Where(t => t.StockNum > 0).GroupBy(t => t.SkuId).Select(m => new
                {
                    skuId = m.Key,
                    skuName = m.First().SkuName
                }),
                dataList = model.SubProduct.Select(x => new
                {
                    baseId = model.BaseId,
                    baseName = model.BaseName,
                    productId = x.ProductId,
                    productName = x.ProductName,
                    productNo = x.ProductNo,
                    Category = model.CategoryId.ToString(),
                    productStocks = x.ProductStocks
                })
            };
            Console.WriteLine("搜索产品测试结果:");
            Console.WriteLine(JsonHelper.Encode(jsonData));
        }


        public static void CreateProductStockTest()
        {
            var productId = 9;
            var sizeSku = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(4, "37码"),
                new KeyValuePair<int, string>(5, "38码"),
                new KeyValuePair<int, string>(6, "39码"),
            };
            var result = ProductStockService.CreateProductStock(productId, sizeSku);
            Console.WriteLine("创建产品库存测试结果:{0}", result);
        }
    }
}
