using System.Net.Mail;
using System.Net;
using OrderAPI.Models;

namespace OrderAPI.GmailSender
{
    public class EmailSender : IEmailSender
    {
        void IEmailSender.SendEmail(string email, string recieverName, List<OrderDetails> orderDetails)
        {
            using (var client = new SmtpClient())
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("raison.naresh1311@gmail.com", "winrnznthibsthup");
                using (var message = new MailMessage(
                    from: new MailAddress("raison.naresh1311@gmail.com", "Naresh"),
                    to: new MailAddress(email, recieverName)
                    ))
                {

                    message.Subject = "Order successfully placed!";
                    message.Body = orderDetails.ToString();

                    client.Send(message);
                }
            }
        }
    }
}
