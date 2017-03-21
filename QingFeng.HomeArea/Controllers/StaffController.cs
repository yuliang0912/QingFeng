using System.Linq;
using QingFeng.Common;
using QingFeng.WebArea.Fillter;
using System.Web.Mvc;
using QingFeng.Business;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.Administrator)]
    public class StaffController : CustomerController
    {
        // GET: Staff
        public ActionResult Index(string keyWords = "")
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.Staff, keyWords).ToList();

            ViewBag.keyWords = keyWords;

            return View(list);
        }
    }
}