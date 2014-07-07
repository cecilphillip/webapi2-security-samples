﻿using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using Owin;
using Thinktecture.IdentityModel;

[assembly: OwinStartup(typeof(WebApiClaimsAuthorization.Startup))]

namespace WebApiClaimsAuthorization
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/Home/Login"),
                SlidingExpiration = true,
                CookieHttpOnly = true,  // already defaults to true
                CookieSecure = CookieSecureOption.SameAsRequest,
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = ctx =>
                    {
                        if (!ctx.Request.IsAjaxRequest())
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                    }
                }
            });

            app.UseResourceAuthorization(new CustomAuthorizationManager());
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

            app.Map("/api", api => api.UseWebApi(config));

        }
    }
}