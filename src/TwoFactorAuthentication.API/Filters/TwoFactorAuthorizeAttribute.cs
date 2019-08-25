using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TwoFactorAuthentication.API.Helpers;

namespace TwoFactorAuthentication.API.Controllers
{
    public class TwoFactorAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            var preSharedKey = principal?.FindFirst("PSK").Value;
            bool hasValidTotp = actionContext.Request.HasValidTotp(preSharedKey);

            if (hasValidTotp)
            {
                return Task.FromResult<object>(null);
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new CustomError() { Code = 100, Message = "Time sensitive passcode is invalid" });
                return Task.FromResult<object>(null);
            }
        }
    }
}