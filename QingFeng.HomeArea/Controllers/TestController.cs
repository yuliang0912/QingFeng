using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remotion.Data.Linq;
using LinqToExcel;
using CiWong.Framework.Captcha;

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


        [CaptchaValidation("login")]
        public JsonResult SaveUser(bool captchaValid)
        {
            return null;
        }
    }


    public class Pmodel
    {
        public int goods_id { get; set; }
        public int sku_id { get; set; }
    }
}