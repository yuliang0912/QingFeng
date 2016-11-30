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
                name: "orderList",
                url: "agent/confirmed",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.待发货.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "orderList1",
                url: "agent/pay",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.已支付.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "orderList2",
                url: "agent/unpay",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.待支付.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "orderList3",
                url: "agent/done",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.已完成.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "orderList4",
                url: "agent/exceptional",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.异常.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "orderList5",
                url: "agent/canceled",
                defaults: new
                {
                    controller = "Agent",
                    action = "OrderList",
                    id = UrlParameter.Optional,
                    orderStatus = AgentEnums.MasterOrderStatus.已取消.GetHashCode()
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}
