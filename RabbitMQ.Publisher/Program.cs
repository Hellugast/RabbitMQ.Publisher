// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");

//Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new("amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo");

//Bağlantı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false);

//Queue mesaj gönderme -- Mesajlar byte türünde olmalı

for (int i = 0; i < 100; i++)
{
    await Task.Delay(100);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}


Console.ReadLine();

