using OrderAPI.Models;

namespace OrderAPI.GmailSender
{
    public interface IEmailSender
    {
        void SendEmail(string email, string firstName, OrderHeader orderHeader);
    }
}
