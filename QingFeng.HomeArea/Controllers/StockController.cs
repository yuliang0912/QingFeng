using ClosedXML.Excel;
using LinqToExcel;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    public class StockController : CustomerController
    {

        public ActionResult Search(int baseId = 0, int brandId = 0, int sexId = 0, int warehouseId = 0, int page = 1,
            int pageSize = 20)
        {
            int totalItem = 0;

            var list = new List<ProductBase>();
            if (baseId > 0)
            {
                var model = ProductService.Instance.GetProductBase(baseId);
                if (model != null)
                {
                    totalItem = 1;
                    list.Add(model);
                }
            }
            else
            {
                list =
                    ProductService.Instance.SearchBaseProduct(brandId, sexId, 0, string.Empty, 0, page, pageSize,
                        out totalItem).ToList();
            }


            var productStocks = ProductStockService.Instance.GetProductStockListByBaseIds(
                list.Select(t => t.BaseId).ToArray())
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
            ViewBag.warehouseId = warehouseId;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }


        public JsonResult SearchGoods(string keyWords)
        {
            if (string.IsNullOrWhiteSpace(keyWords))
            {
                return Json(Enumerable.Empty<object>());
            }

            var list = ProductService.Instance.SearchBaseProduct(0, 0, keyWords).Select(t => new
            {
                baseId = t.BaseId,
                baseNo = t.BaseNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import()
        {
            return View();
        }


        //导入EXCEL数据(IIS-32位运行)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StockFileImport(HttpPostedFileBase file)
        {
            if (string.Empty.Equals(file.FileName) || ".xlsx" != System.IO.Path.GetExtension(file.FileName))
            {
                return Json(new ApiResult<bool>(false)
                {
                    ErrorCode = 1,
                    Message = "当前文件格式不正确,请确保正确的Excel文件格式!"
                });
            }

            int brandId = Convert.ToInt32(Request.Form["brand_id"] ?? "");
            if (brandId == 0)
            {
                return Json(new ApiResult<bool>(false)
                {
                    ErrorCode = 2,
                    Message = "未选择品牌"
                });
            }

            var severPath = Server.MapPath("/content/upload/"); //获取当前虚拟文件路径

            var savePath = System.IO.Path.Combine(severPath, Guid.NewGuid().ToString() + ".xlsx"); //拼接保存文件路径

            try
            {
                file.SaveAs(savePath);

                var execelfile = new ExcelQueryFactory(savePath);

                execelfile.AddMapping<ProductStockExcelDTO>(x => x.BaseId, "商品ID");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.ProductId, "spu_id");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.SkuId, "sku_id");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.BaseNo, "货号");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.ProductNo, "颜色");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.SkuName, "尺码");
                execelfile.AddMapping<ProductStockExcelDTO>(x => x.StockNum, "库存");

                var lineItems = execelfile.Worksheet<ProductStockExcelDTO>(0).ToList();

                var rows = ProductStockService.Instance.ResetProductStock(lineItems);

                return Json(new ApiResult<int>()
                {
                    Data = rows,
                    ErrorCode = 0,
                    Message = $"成功导入{rows}条数据,导入失败{lineItems.Count - rows}条数据"
                });
            }
            catch (Exception e)
            {
                return Json(new ApiResult<bool>(false)
                {
                    Ret = Common.ApiCore.RetEum.ApplicationError,
                    ErrorCode = 3,
                    Message = "导入EXCEL失败,请严格按照模板导入,系统错误信息:" + e.ToString()
                });
            }
            finally
            {
                System.IO.File.Delete(savePath);//每次上传完毕删除文件
            }
        }

        //导出模板文件
        public ActionResult StockExportExcel(int brandId)
        {
            int totalItem;
            var excelDataList = new List<ProductStockExcelDTO>();

            var list = ProductService.Instance.SearchBaseProduct(brandId, 0, 0, string.Empty, 0, 1, int.MaxValue, out totalItem);

            var productStocks = ProductStockService.Instance.GetProductStockListByBaseIds(list.Select(t => t.BaseId).ToArray())
                .GroupBy(t => t.ProductId)
                .ToDictionary(c => c.Key, c => c.GroupBy(t => t.SkuId).ToDictionary(a => a.Key, a => a.First()));

            var productSkus = ProductService.Instance.GetProductSkuListByBaseIds(list.Select(t => t.BaseId).ToArray())
               .GroupBy(c => c.ProductId).ToDictionary(c => c.Key, c => c.ToList());

            foreach (var item in list.SelectMany(x => x.SubProduct))
            {
                if (!productSkus.ContainsKey(item.ProductId))
                {
                    continue;
                }
                foreach (var sku in productSkus[item.ProductId])
                {
                    var model = new ProductStockExcelDTO()
                    {
                        BaseId = item.BaseId,
                        BaseNo = item.BaseNo,
                        ProductId = item.ProductId,
                        ProductNo = item.ProductNo,
                        SkuId = sku.SkuId,
                        SkuName = sku.SkuName,
                    };
                    if (productStocks.ContainsKey(item.ProductId))
                    {
                        if (productStocks[item.ProductId].ContainsKey(sku.SkuId))
                        {
                            model.StockNum = productStocks[item.ProductId][sku.SkuId].StockNum;
                        }
                    }
                    excelDataList.Add(model);
                }
            }

            var jsonStr = JsonHelper.Encode(excelDataList);

            var dataTable = JsonHelper.Decode<DataTable>(jsonStr);

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(dataTable, "Sheet1");

            var workSheet = workbook.Worksheet(1);
            workSheet.Rows(1, 1000).Height = 20;
            workSheet.Columns(1, 100).Width = 25;
            workSheet.Range("A1:G1").Style.Fill.BackgroundColor = XLColor.Green;
            workSheet.Range("A1:G1").Style.Font.SetFontColor(XLColor.Yellow);
            workSheet.Range("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            return new Common.ActionResultExtensions.ExportExcelResult
            {
                workBook = workbook,
                FileName = string.Concat("goods-stock-template-", DateTime.Now.ToString("yyyy-MM-dd"), ".xlsx")
            };
        }
    }
}