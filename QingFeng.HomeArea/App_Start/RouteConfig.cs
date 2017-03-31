using System.Web.Mvc;
using System.Web.Routing;
using QingFeng.Common;
using System.Collections.Generic;

namespace QingFeng.WebArea
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var orderRouteList = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.待发货.GetHashCode(), "confirmed"),
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.待支付.GetHashCode(), "unpay"),
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.已完成.GetHashCode(), "done"),
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.已发货.GetHashCode(), "send"),
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.异常.GetHashCode(), "exceptional"),
                new KeyValuePair<int, string>(AgentEnums.MasterOrderStatus.已取消.GetHashCode(), "canceled")
            };

            orderRouteList.ForEach(item =>
            {
                routes.MapRoute("orderList" + item.Key, "order/" + item.Value, new
                {
                    controller = "Order",
                    action = "Index",
                    id = UrlParameter.Optional,
                    orderStatus = item.Key
                });
            });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new {controller = "Home", action = "Welcome"}
            );
        }
    }
}
