using ClosedXML.Excel;
using LinqToExcel;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.Models.DTO;
using QingFeng.WebArea.Fillter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    public class PriceController : CustomerController
    {
        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商价格)]
        public ActionResult Index(int userId = 0, string keyWord = "", int brandId = 1, int categoryId = 0, int page = 1,
            int pageSize = 30)
        {
            var totalItem = 0;
            var list = new List<ProductBase>();
            var userPrice = new Dictionary<int, UserProductPrice>();

            if (userId > 0)
            {
                list = ProductService.Instance.SearchBaseProduct(0, 0, categoryId, keyWord, 0, page, pageSize, out totalItem).ToList();

                userPrice = UserService.Instance.GetUserPrice(userId, brandId, list.Select(t => t.BaseId).ToArray())
                    .ToDictionary(c => c.ProductId, c => c);
            }

            var allUsers = UserService.Instance.GetList(new { UserRole = 3 }).ToList();

            ViewBag.brandId = brandId;
            ViewBag.categoryId = categoryId;
            ViewBag.keyWord = keyWord;
            ViewBag.userId = userId;
            ViewBag.userPrice = userPrice;
            ViewBag.userList = allUsers;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.设置代理商价格)]
        //设置价格
        public ActionResult DistributorSet(int userId, int baseId)
        {
            var baseInfo = ProductService.Instance.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }

            var storeUser = UserService.Instance.GetUserInfo(new {userId, userRole = AgentEnums.UserRole.StoreUser});

            if (storeUser == null)
            {
                return Content("参数错误");
            }

            
            ViewBag.userPrice =
                UserService.Instance.GetUserPrice(userId, baseInfo.BrandId.GetHashCode(), baseInfo.BaseId)
                    .ToDictionary(c => c.ProductId, c => c);

            ViewBag.storeUser = storeUser;

            return View(baseInfo);
        }

        [HttpPost]
        [AdminAuthorize(AgentEnums.SubMenuEnum.设置代理商价格)]
        public ActionResult SetUserPrice(int userId, int baseId)
        {
            var baseInfo = ProductService.Instance.GetProductBase(baseId);
            if (baseInfo == null)
            {
                return Content("参数错误");
            }

            var storeUser = UserService.Instance.GetUserInfo(new {userId, userRole = AgentEnums.UserRole.StoreUser});
            if (storeUser == null)
            {
                return Content("参数错误");
            }

            var list = (from item in baseInfo.SubProduct
                let cosePrice = Convert.ToDecimal(Request.Form[$"cost_{item.ProductId}"])
                where cosePrice > 0
                select new UserProductPrice()
                {
                    BaseId = item.BaseId,
                    BrandId = baseInfo.BrandId,
                    ProductId = item.ProductId,
                    UserId = userId,
                    OriginalPrice = item.OriginalPrice,
                    ActualPrice = cosePrice,
                    UpdateDate = DateTime.Now
                }).ToList();

            var result = UserService.Instance.ResetUserPrice(userId, baseInfo.BaseId, list);

            return Json(result);
        }

        //批量EXCEL导入
        [HttpGet]
        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商导入)]
        public ActionResult DistributorImport()
        {
            var allUsers = UserService.Instance.GetList(new { UserRole = 3 }).ToList();

            return View(allUsers);
        }


        //导入EXCEL数据(IIS-32位运行)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商导入)]
        public JsonResult DistributorFileImport(HttpPostedFileBase file)
        {
            if (string.Empty.Equals(file.FileName) || ".xlsx" != System.IO.Path.GetExtension(file.FileName))
            {
                return Json(new ApiResult<bool>(false)
                {
                    ErrorCode = 1,
                    Message = "当前文件格式不正确,请确保正确的Excel文件格式!"
                });
            }

            int userId = Convert.ToInt32(Request.Form["user_id"] ?? "");
            int brandId = Convert.ToInt32(Request.Form["brand_id"] ?? "");
            if (userId == 0 || brandId == 0)
            {
                return Json(new ApiResult<bool>(false)
                {
                    ErrorCode = 2,
                    Message = "未选择代理商或者品牌"
                });
            }

            var severPath = Server.MapPath("/content/upload/"); //获取当前虚拟文件路径

            var savePath = System.IO.Path.Combine(severPath, Guid.NewGuid().ToString() + ".xlsx"); //拼接保存文件路径

            try
            {
                file.SaveAs(savePath);

                var execelfile = new ExcelQueryFactory(savePath);

                execelfile.AddMapping<UserPriceExcelDTO>(x => x.BaseId, "商品ID");
                execelfile.AddMapping<UserPriceExcelDTO>(x => x.ProductId, "spu_id");
                execelfile.AddMapping<UserPriceExcelDTO>(x => x.BaseNo, "货号");
                execelfile.AddMapping<UserPriceExcelDTO>(x => x.ProductNo, "颜色");
                execelfile.AddMapping<UserPriceExcelDTO>(x => x.OriginalPrice, "市场价");
                execelfile.AddMapping<UserPriceExcelDTO>(x => x.ActualPrice, "供货价");

                var lineItems = execelfile.Worksheet<UserPriceExcelDTO>(0).ToList();

                if (lineItems.Any(t => t.ActualPrice < 0 || t.OriginalPrice < 0))
                {
                    return Json(new ApiResult<bool>(false)
                    {
                        ErrorCode = 4,
                        Message = "价格不能设置成低于0元"
                    });
                }

                var rows = UserService.Instance.ResetUserPrice(userId, brandId, lineItems);

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
        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商导入)]
        public ActionResult DistributorImportExcel(int brandId, int userId)
        {
            var baseProductList = ProductService.Instance.GetBaseProductList(new { brandId, status = 0 });

            var userPrice = new Dictionary<int, UserProductPrice>();

            if (userId > 0)
            {
                userPrice = UserService.Instance.GetUserPrice(userId, brandId, baseProductList.Select(t => t.BaseId).ToArray())
                    .ToDictionary(c => c.ProductId, c => c);
            }

            var excelDataList = baseProductList.SelectMany(t => t.SubProduct).Where(t => t.Status == 0).OrderBy(t => t.BaseId).Select(t => new UserPriceExcelDTO()
            {
                BaseId = t.BaseId,
                BaseNo = t.BaseNo,
                ProductId = t.ProductId,
                ProductNo = t.ProductNo,
                OriginalPrice = t.OriginalPrice,
                ActualPrice = userPrice.ContainsKey(t.ProductId) ? userPrice[t.ProductId].ActualPrice : t.ActualPrice
            }).ToList();

            var jsonStr = JsonHelper.Encode(excelDataList);

            var dataTable = JsonHelper.Decode<DataTable>(jsonStr);

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(dataTable, "Sheet1");

            var workSheet = workbook.Worksheet(1);
            workSheet.Rows(1, 1000).Height = 20;
            workSheet.Columns(1, 100).Width = 25;
            workSheet.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.Green;
            workSheet.Range("A1:F1").Style.Font.SetFontColor(XLColor.Yellow);
            workSheet.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            return new Common.ActionResultExtensions.ExportExcelResult
            {
                workBook = workbook,
                FileName = string.Concat("goods-agent-template-", DateTime.Now.ToString("yyyy-MM-dd"), ".xlsx")
            };
        }
    }
}