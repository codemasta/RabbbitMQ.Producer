using System;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

namespace RabbbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
          
            QueueProducer.DirectExchangePublish(channel);

            Console.ReadLine();
        }
    }
}
