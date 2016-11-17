using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Models;

namespace QingFeng.WebArea.Controllers
{
    public class AgentController : Controller
    {
        private readonly OrderService _orderService = new OrderService();
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrder(OrderMaster order)
        {
            if (_orderService.IsExists(order.OrderNo))
            {

            }
            return null;
        }
    }
}