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
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Content("参数错误");
            }

            return View(userInfo);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.添加店铺)]
        public ActionResult Add()
        {
            return View();
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.代理商列表)]
        public ActionResult Agent()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.重置代理商密码)]
        public ActionResult ReSetAgentPwd()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.删除代理商)]
        public ActionResult SetAgentLogin()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        #region Ajax

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult Edit()
        {
            return Json(null);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult ProhibitLogin()
        {
            return Json(null);
        }

        #endregion
    }
}