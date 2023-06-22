// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");


ConnectionFactory factory = new();
factory.Uri = new("amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        routingKey: string.Empty,
        body: message);
}


Console.ReadLine();