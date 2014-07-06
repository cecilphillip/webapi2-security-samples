using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(WebApiCors.Startup))]

namespace WebApiCors
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}"
            );

            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            var cors = new EnableCorsAttribute("http://localhost:3054", "*", "*");
            config.EnableCors(cors);

            app.Map("/api", api =>
            {              
                api.UseWebApi(config);
            });
        }
    }
}
