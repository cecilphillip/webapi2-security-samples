using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(OwinCors.Startup))]

namespace OwinCors
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

            app.Map("/api", api =>
            {
                var corsOptions = new CorsOptions()
                {
                    PolicyProvider = new CorsPolicyProvider
                    {
                        PolicyResolver = ctx =>
                        {
                            var policy = new CorsPolicy();
                            policy.Origins.Add("http://localhost:3054");
                            policy.AllowAnyHeader = true;
                            policy.Methods.Add("GET");
                            return Task.FromResult(policy);
                        }
                    }

                };
                api.UseCors(corsOptions);
                api.UseWebApi(config);
            });
        }
    }
}
