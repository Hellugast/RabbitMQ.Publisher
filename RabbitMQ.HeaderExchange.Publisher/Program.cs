// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");


ConnectionFactory factory = new();
factory.Uri = new("amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo");


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers
    );

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    Console.WriteLine("Header value giriniz");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>()
    {
        ["no"] = value
    };

    channel.BasicPublish(
        exchange: "header-exchange-example",
        routingKey: string.Empty,
        body: message,
        basicProperties: basicProperties
        );
}


Console.ReadLine();

