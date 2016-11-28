using System.Web.Mvc;
using System.Web.Routing;
using QingFeng.Common;

namespace QingFeng.WebArea
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "orderList",
                url: "confirmed", //待发货
                defaults:
                new
                {
                    controller = "Agent",
                    action = "OrderList",
                    orderStatus = AgentEnums.MasterOrderStatus.待发货.GetHashCode()
                },
                constraints: new RouteValueDictionary
                {
                    {"page", @"^\d{1,}$"}
                }
            );
        }
    }
}
