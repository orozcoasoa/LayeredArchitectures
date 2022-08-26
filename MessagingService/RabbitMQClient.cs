using System.Text;
using System.Text.Json;
using MessagingService.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagingService
{
    public class RabbitMQClient : IMQClient, IDisposable
    {
        private readonly string exchangeName = "items";
        private readonly string itemUpdateRoutingKey = "items.update";
        private readonly string itemDeleteRoutingKey = "items.delete";
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(IConnection connection)
        {
            _connection = connection;
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;
            _channel = _connection.CreateModel();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ connection shutdown...");
        }

        public void PublishItemDeleted(int id)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            var itemToDelete = new Item() { Id = id };
            var message = JsonSerializer.Serialize(itemToDelete);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchangeName, itemDeleteRoutingKey, null, body);
        }
        public void PublishItemUpdated(Item item)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            var message = JsonSerializer.Serialize(item);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchangeName, itemUpdateRoutingKey, null, body);
        }
        public void SubscribeToItemDeletes(Func<int, bool> action)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, exchangeName, itemDeleteRoutingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var item = JsonSerializer.Deserialize<Item>(message);
                var result = action.Invoke(item.Id);
                if (result)
                    _channel.BasicAck(ea.DeliveryTag, false);
                else
                {
                    //TODO: process potential failure
                }
            };
            _channel.BasicConsume(queueName, false, consumer);
        }
        public void SubscribeToItemUpdates(Func<Item, bool> action)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, exchangeName, itemUpdateRoutingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var item = JsonSerializer.Deserialize<Item>(message);
                var result = action.Invoke(item);
                if (result)
                    _channel.BasicAck(ea.DeliveryTag, false);
                else
                {
                    //TODO: process potential failure
                }
            };
            _channel.BasicConsume(queueName, false, consumer);
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
                _channel.Dispose();
                _connection.Dispose();
            }

        }
    }
}
