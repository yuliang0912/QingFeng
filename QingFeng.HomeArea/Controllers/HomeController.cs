using System.Web.Mvc;
using QingFeng.Business.MenuService;

namespace QingFeng.WebArea.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index1()
        {
            var model = new MenuService().Get();
            return View();
        }
    }
}