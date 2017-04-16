using ClosedXML.Excel;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.Common.ActionResultExtensions
{
    public class ExportExcelResult : ActionResult
    {

        public XLWorkbook workBook { get; set; }

        public string FileName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            ExportExcelEventHandler(context);
        }

        /// <summary>
        /// Exports the excel event handler.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ExportExcelEventHandler(ControllerContext context)
        {
            context.HttpContext.Response.Clear();

            // 编码
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;

            // 设置网页ContentType
            context.HttpContext.Response.ContentType =
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // 导出名字
            var browser = context.HttpContext.Request.Browser.Browser;
            var exportFileName = browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase)
                ? FileName
                : HttpUtility.UrlEncode(this.FileName, Encoding.UTF8);

            context.HttpContext.Response.AddHeader(
                "Content-Disposition", $"attachment;filename={exportFileName}");

            using (var memoryStream = new MemoryStream())
            {
                workBook.SaveAs(memoryStream);
                memoryStream.WriteTo(context.HttpContext.Response.OutputStream);
                memoryStream.Close();
            }
            workBook.Dispose();
        }
    }
}