using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TwoFactorAuthentication.API;

[assembly: OwinStartup(typeof(Startup))]

namespace TwoFactorAuthentication.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            OAuthBearerAuthenticationOptions oAuthBearerOptions=new OAuthBearerAuthenticationOptions();
            OAuthAuthorizationServerOptions oAuthServerOptions=new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            //Token Consumption
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);
            
        }
    }
}
