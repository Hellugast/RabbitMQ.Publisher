// See https://aka.ms/new-console-template for more information
using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;

Console.WriteLine("Hello, World!");

string rabbitMQUri = "amqps://lhlfzvgo:***@crow.rmq.cloudamqp.com/lhlfzvgo";
string requestQueue = "request-queue";

var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(rabbitMQUri);
});

await bus.StartAsync();

var request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));

int i = 0;
while (true)
{
    await Task.Delay(200);
    var response = await request.GetResponse<ResponseMessage>(new() { MessageNo = i, Text = $"{i++}. request" });
    Console.WriteLine($"Response received : {response.Message.Text}");
}

Console.ReadLine();
