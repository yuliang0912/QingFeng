using QingFeng.Business;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pay(UserInfo user, long orderId)
        {

            var orderInfo = OrderService.Instance.Get(new { orderId });

            if (orderInfo == null)
            {
                return Content("未找到订单");
            }

            if (orderInfo.OrderStatus != Common.AgentEnums.MasterOrderStatus.待支付)
            {
                return Content("只有待支付的订单才能支付");
            }

            if(orderInfo.UserId != user.UserId)
            {
                return Content("只能支付自己的订单");
            }

            return Redirect("");
        }
    }
}