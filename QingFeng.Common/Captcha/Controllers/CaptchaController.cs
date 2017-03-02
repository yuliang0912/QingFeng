using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using CiWong.Framework.Captcha; 

namespace CiWong.Framework.Captcha.Controllers
{
    public class CaptchaController : Controller
    {
        //
        // GET: /Captcha/ID
        public ActionResult Index(string aid)
        {
            CaptchaImage ci = CaptchaImage.CreateCaptcha(aid);
            using (Bitmap b = ci.RenderImage())
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
