using OrderAPI.Messages;

namespace OrderAPI.Messaging
{
    public interface IRabbitMqConsumer
    {
         void SubscribeMessage();
    }
}
