using System.Linq;
using QingFeng.Common;
using QingFeng.WebArea.Fillter;
using System.Web.Mvc;
using QingFeng.Business;

namespace QingFeng.WebArea.Controllers
{
    public class StaffController : CustomerController
    {
        // GET: Staff
        [AdminAuthorize(AgentEnums.SubMenuEnum.员工列表)]
        public ActionResult Index(string keyWords = "")
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.Staff, keyWords).ToList();

            ViewBag.keyWords = keyWords;

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.编辑店铺)]
        public ActionResult Edit(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new { userId = userId });

            return View(userInfo);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.添加店铺)]
        public ActionResult Add()
        {
            return View();
        }
    }
}