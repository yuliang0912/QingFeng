using System;
using System.Linq;
using QingFeng.Common;
using QingFeng.WebArea.Fillter;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Models;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Common.ApiCore;

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

        [AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public ActionResult Edit(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Content("参数错误");
            }

            return View(userInfo);
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.添加员工)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet, AdminAuthorize(AgentEnums.SubMenuEnum.代理商列表)]
        public ActionResult Agent()
        {
            var list = UserService.Instance.Search(AgentEnums.UserRole.StoreUser, string.Empty).ToList();

            return View(list);
        }

        [HttpGet, AdminAuthorize(AgentEnums.SubMenuEnum.添加代理商)]
        public ActionResult AddAgent()
        {
            return View();
        }

        [HttpGet, AdminAuthorize(AgentEnums.SubMenuEnum.编辑代理商)]
        public ActionResult EditAgent(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Content("参数错误");
            }

            return View(userInfo);
        }

        [HttpGet, AdminAuthorize(AgentEnums.SubMenuEnum.重置代理商密码)]
        public ActionResult ReSetAgentPwd(int userId)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Content("参数错误");
            }

            return View(userInfo);
        }

        #region Ajax

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.添加员工)]
        public JsonResult AddStaff(UserInfo model)
        {
            model.CreateDate = DateTime.Now;
            model.Status = 0;
            model.UserRole = AgentEnums.UserRole.Staff;
            model.UserMenus = string.Empty;
            //"101,102,103,104,201,202,203,204,205,206,207,208,209,210,301,302,303,304,305,401,402,403,404,501,502,503,504,505,506,507,508,509,510,511,512,513,514,515,516,517,518,601,602,603";

            var result = UserService.Instance.RegisterUser(model);
            return Json(result);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑员工)]
        public JsonResult EditStaff(UserInfo model)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {model.UserId});

            if (userInfo == null || userInfo.UserRole != AgentEnums.UserRole.Staff)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "参数错误"});
            }
            userInfo.Phone = model.Phone;
            userInfo.Email = model.Email;
            userInfo.NickName = model.NickName;
            if (!string.IsNullOrWhiteSpace(model.PassWord))
            {
                userInfo.PassWord = model.PassWord;
            }
            var result = UserService.Instance.UpdateUserInfo(userInfo);
            return Json(result);
        }


        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.添加代理商)]
        public JsonResult AddAgent(UserInfo model)
        {
            model.CreateDate = DateTime.Now;
            model.Status = 0;
            model.UserRole = AgentEnums.UserRole.StoreUser;
            model.UserMenus = "501,504,508,518"; //查看订单,订单导出,取消订单,添加订单

            var result = UserService.Instance.RegisterUser(model);
            return Json(result);
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.编辑代理商)]
        public JsonResult EditAgent(UserInfo model)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {model.UserId});

            if (userInfo == null || userInfo.UserRole != AgentEnums.UserRole.StoreUser)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "参数错误"});
            }
            userInfo.Phone = model.Phone;
            userInfo.Email = model.Email;
            userInfo.NickName = model.NickName;
            if (!string.IsNullOrWhiteSpace(model.PassWord))
            {
                userInfo.PassWord = model.PassWord;
            }
            var result = UserService.Instance.UpdateUserInfo(userInfo);
            return Json(result);
        }


        [HttpGet, MenuAuthorize(AgentEnums.SubMenuEnum.编辑员工, AgentEnums.SubMenuEnum.删除代理商)]
        public JsonResult ProhibitLogin(UserInfo user, int userId)
        {
            if (user.UserId == userId)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "不能更新自己的状态"});
            }

            var result = UserService.Instance.Update(new {Status = 1}, new {userId});

            return Json(new ApiResult<bool>(result));
        }

        [HttpGet, MenuAuthorize(AgentEnums.SubMenuEnum.编辑员工, AgentEnums.SubMenuEnum.删除代理商)]
        public JsonResult AllowLogin(UserInfo user, int userId)
        {
            if (user.UserId == userId)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "不能更新自己的状态"});
            }

            var result = UserService.Instance.Update(new {Status = 0}, new {userId});

            return Json(new ApiResult<bool>(result));
        }

        [HttpPost, AdminAuthorize(AgentEnums.SubMenuEnum.重置代理商密码)]
        public ActionResult ReSetAgentPwd(int userId, string passWord)
        {
            var userInfo = UserService.Instance.GetUserInfo(new {userId = userId});

            if (null == userInfo)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "参数错误"});
            }

            var result = UserService.Instance.UpdatePassWord(userInfo, passWord);

            return Json(result);
        }

        #endregion
    }
}