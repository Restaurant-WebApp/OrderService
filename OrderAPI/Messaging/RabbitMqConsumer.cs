using Newtonsoft.Json;
using OrderAPI.GmailSender;
using OrderAPI.Messages;
using OrderAPI.Models;
using OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace OrderAPI.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IOrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private readonly IEmailSender _emailSender;
                
        public RabbitMqConsumer(IOrderRepository repository, IEmailSender emailsender)
        {
            _repository = repository;
            _emailSender = emailsender;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null); //Type of channel
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evnt) =>
            {
                var content = Encoding.UTF8.GetString(evnt.Body.ToArray());
                CheckoutHeader checkoutHeader = JsonConvert.DeserializeObject<CheckoutHeader>(content);
                HandleMessage(checkoutHeader).GetAwaiter().GetResult();

                _channel.BasicAck(evnt.DeliveryTag, false);

            };
            _channel.BasicConsume("checkoutqueue", false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CheckoutHeader checkoutHeader)
        {
            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeader.UserId,
                FirstName = checkoutHeader.FirstName,
                LastName = checkoutHeader.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeader.CardNumber,                
                CVV = checkoutHeader.CVV,
                Email = checkoutHeader.Email,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeader.OrderTotal,
                PhoneNumber = checkoutHeader.PhoneNumber,
            };
            foreach (var detailList in checkoutHeader.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _repository.AddOrder(orderHeader);
            _emailSender.SendEmail(orderHeader.Email, orderHeader.FirstName, orderHeader.OrderDetails);

            try
            {

            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
