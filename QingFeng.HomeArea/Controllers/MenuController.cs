using System.Linq;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.设置权限)]
        public ActionResult SetRole(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId});
            ViewBag.userRoles = string.Join(",", userInfo.AllUserMenus.Select(t => t.GetHashCode()));
            return View();
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.设置权限)]
        public JsonResult SetUserRole(int userId)
        {
            var userRoles = Request.Form["userRoles"] ?? string.Empty;
            return Json(true);
        }
    }
}