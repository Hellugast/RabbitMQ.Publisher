// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");


ConnectionFactory factory = new();
factory.Uri = new("amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P(point to point) Tasarımı
//string queueName = "example-p2p-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//byte[] message = Encoding.UTF8.GetBytes("merhaba");

//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message);

#endregion
#region Publish/Subscribe (Pub/Sub) Tasarımı
//string exchangeName = "example-pubsub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: message);
//}

#endregion
#region Work Queue(İş Kuyruğu) Tasarımı
//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message);
//}

#endregion
#region Request/Response Tasarımı
string requestQueueName = "example-requestresponse-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string responseQueueName = channel.QueueDeclare().QueueName;

string correlationID = Guid.NewGuid().ToString();

//Request Mesajını Oluşturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationID;
properties.ReplyTo = responseQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        basicProperties: properties,
        body: message);
}

//Response Kuyruğu Dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: responseQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationID)
    {
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    }
};

#endregion

Console.Read();