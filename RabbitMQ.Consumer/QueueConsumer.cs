using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public static class QueueConsumer
    {
        public static void DefaultConsume(IModel channel)
        {
            channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }

        public static void DirectExchangeConsume(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            channel.QueueDeclare("demo-direct-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind("demo-direct-queue", "demo-direct-exchange", "account.init");
            channel.BasicQos(0, 10, false); // each consumer will fetch 10 messages at a time

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-direct-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }

        public static void TopicExchangeConsumer(IModel channel)
        {
            channel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic);
            channel.QueueDeclare("demo-topic-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind("demo-topic-queue", "demo-topic-exchange", "account.*"); // uses pattern matching
            channel.BasicQos(0, 10, false); // each consumer will fetch 10 messages at a time

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-topic-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }

        public static void HeaderExchangeConsumer(IModel channel)
        {
            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Headers);
            channel.QueueDeclare("demo-header-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var header = new Dictionary<string, object> { { "account", "new" } };

            channel.QueueBind("demo-header-queue", "demo-header-exchange", string.Empty , header); 
            channel.BasicQos(0, 10, false); // each consumer will fetch 10 messages at a time

           var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-header-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }

        public static void FanOutExchangeConsumer(IModel channel)
        {
            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout);
            channel.QueueDeclare("demo-fanout-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

           
            channel.QueueBind("demo-fanout-queue", "demo-fanout-exchange", string.Empty);
            channel.BasicQos(0, 10, false); // each consumer will fetch 10 messages at a time

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("demo-fanout-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}