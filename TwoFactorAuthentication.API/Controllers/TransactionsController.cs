using System.Web.Http;

namespace TwoFactorAuthentication.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : ApiController
    {

        [Route("history")]
        public IHttpActionResult GetHistory()
        {
            return Ok(Transaction.CreateTransactions());
        }

        [Route("transfer")]
        [TwoFactorAuthorize]
        public IHttpActionResult PostTransfer(TransferModeyModel transferModeyModel)
        {
            return Ok();
        }
    }
}
