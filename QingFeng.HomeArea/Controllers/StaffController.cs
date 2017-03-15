using QingFeng.Common;
using QingFeng.WebArea.Fillter;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.Administrator)]
    public class StaffController : CustomerController
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }
    }
}