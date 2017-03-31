using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace QingFeng.Common.Captcha.Controllers
{
    public class CaptchaController : Controller
    {
        //
        // GET: /Captcha/ID
        public ActionResult Index(string aid)
        {
            var ci = CaptchaImage.CreateCaptcha(aid);
            using (var b = ci.RenderImage())
            {
                b.Save(Response.OutputStream, ImageFormat.Jpeg);
            }
            Response.ContentType = "image/jpeg";
            Response.StatusCode = 200;
            Response.StatusDescription = "OK";
            return null;            
        }


       

    }
}
