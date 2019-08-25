namespace TwoFactorAuthentication.API.Controllers
{
    public class TransferModeyModel
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public double Amount { get; set; }
    }
}