using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using TwoFactorAuthentication.API.Models;

namespace TwoFactorAuthentication.API
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var  allowedOrigin = "*";
            ApplicationUser appUser = null;

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            using (AuthRepository repo = new AuthRepository())
            {
                appUser = await repo.FindUser(context.UserName, context.Password);

                if (appUser == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
            identity.AddClaim(new Claim("PSK", appUser.Psk));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { 
                    "userName", context.UserName
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}