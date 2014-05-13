using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace TestWebFormsApp
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.EnableFriendlyUrls();
        }
    }
}