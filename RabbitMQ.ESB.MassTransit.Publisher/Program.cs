// See https://aka.ms/new-console-template for more information
using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

Console.WriteLine("Hello, World!");


string rabbitMQUri = "amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo";
string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.WriteLine("Gönderilecek mesaj : ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
});

Console.ReadLine();