using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remotion.Data.Linq;
using LinqToExcel;
using CiWong.Framework.Captcha;
using Newtonsoft.Json;
using System.Data;
using QingFeng.Common.ActionResultExtensions;
using ClosedXML.Excel;

namespace QingFeng.WebArea.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            var path = Server.MapPath("~/content/upload/test.csv");
            var execelfile = new ExcelQueryFactory(path);

            execelfile.AddMapping<Pmodel>(x => x.goods_id, "goods_id");
            execelfile.AddMapping<Pmodel>(x => x.sku_id, "sku_id");

            var lineItems = from oli in execelfile.Worksheet<Pmodel>("Sheet1") select oli;

            var result = 0;
            foreach (var lineItem in lineItems)
            {
                result += lineItem.sku_id;
            }

            return Content(result.ToString());
        }

        public ActionResult export()
        {
            var list = new List<Pmodel>();
            list.Add(new Pmodel { goods_id = 1, sku_id = 1, size = 1 });
            list.Add(new Pmodel { goods_id = 2, sku_id = 2, size = 2 });
            list.Add(new Pmodel { goods_id = 3, sku_id = 3, size = 3 });

            var jsonStr = JsonConvert.SerializeObject(list);

            var dataTable = JsonConvert.DeserializeObject<DataTable>(jsonStr);

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(dataTable, "Sheet1");

            var workSheet = workbook.Worksheet(0);
            workSheet.Rows(1, 1000).Height = 20;
            workSheet.Columns(1, 100).Width = 25;
            workSheet.Range("A1:C1").Style.Fill.BackgroundColor = XLColor.Yellow;
            workSheet.Range("A1:C1").Style.Font.SetFontColor(XLColor.Yellow);
            workSheet.Range("A1:C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            return new ExportExcelResult
            {
                workBook = workbook,
                FileName = string.Concat("ExportData_", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx")
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Browse(HttpPostedFileBase file)
        {
            if (string.Empty.Equals(file.FileName) || ".xlsx" != System.IO.Path.GetExtension(file.FileName))
            {
                throw new ArgumentException("当前文件格式不正确,请确保正确的Excel文件格式!");
            }

            var severPath = this.Server.MapPath("/content/uoload/"); //获取当前虚拟文件路径

            var savePath = System.IO.Path.Combine(severPath, Guid.NewGuid().ToString() + ".xlsx"); //拼接保存文件路径

            try
            {
                file.SaveAs(savePath);

                var execelfile = new ExcelQueryFactory(savePath);

                execelfile.AddMapping<Pmodel>(x => x.goods_id, "标题1");
                execelfile.AddMapping<Pmodel>(x => x.sku_id, "标题2");
                execelfile.AddMapping<Pmodel>(x => x.size, "标题3");

                var lineItems = execelfile.Worksheet<Pmodel>(0).ToList();

                var result = 0;
                foreach (var lineItem in lineItems)
                {
                    result += lineItem.sku_id;
                }
            }
            finally
            {
                System.IO.File.Delete(savePath);//每次上传完毕删除文件
            }
            return View("Index");
        }

        [CaptchaValidation("login")]
        public JsonResult SaveUser(bool captchaValid)
        {
            return null;
        }
    }


    public class Pmodel
    {
        [JsonProperty(PropertyName = "哈哈")]
        public int goods_id { get; set; }

        [JsonProperty(PropertyName = "呵呵")]
        public int sku_id { get; set; }

        [JsonProperty(PropertyName = "嘿嘿")]
        public float size { get; set; }

    }
}