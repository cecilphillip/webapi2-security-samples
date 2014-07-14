using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using WebApiKeys;

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiKeys
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

            config.MessageHandlers.Add(new ApiKeyHeaderHandler());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            app.Map("/api", api => api.UseWebApi(config));
        }
    }
}
