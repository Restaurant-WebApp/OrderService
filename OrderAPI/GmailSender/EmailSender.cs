using System.Net.Mail;
using System.Net;
using System.Text;
using OrderAPI.Messages;
using OrderAPI.Models;

namespace OrderAPI.GmailSender
{
    public class EmailSender : IEmailSender
    {
        
        void IEmailSender.SendEmail(string email, string recieverName, OrderHeader orderHeader)
        {
            
            using (var client = new SmtpClient())
            {
                List<OrderDetails> productList = orderHeader.OrderDetails;
                 
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
                    StringBuilder bodyBuilder = new StringBuilder();
                    bodyBuilder.AppendLine("Products:");

                    foreach (OrderDetails orderDetail in productList)
                    {
                        bodyBuilder.AppendLine($"Product Name: {orderDetail.ProductName}");
                        bodyBuilder.AppendLine($"Price: {orderDetail.Price}");
                        bodyBuilder.AppendLine($"Count: {orderDetail.Count}");
                        bodyBuilder.AppendLine();
                    }
                    message.Subject = "Order successfully placed!";
                    message.Body = bodyBuilder.ToString();

                    client.Send(message);
                }
            }
        }
    }
}
