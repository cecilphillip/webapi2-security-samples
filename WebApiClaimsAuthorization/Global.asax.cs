using System.Web.Routing;

namespace WebApiClaimsAuthorization
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);      
        }
    }
}
