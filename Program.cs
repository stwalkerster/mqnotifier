using System;

namespace MqNotifier
{
    using System.IO;
    using System.Text;
    using RabbitMQ.Client;

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("AMQP_HOSTNAME"),
                Port = int.Parse(Environment.GetEnvironmentVariable("AMQP_PORT")),
                VirtualHost = Environment.GetEnvironmentVariable("AMQP_VHOST"),
                UserName = Environment.GetEnvironmentVariable("AMQP_USERNAME"),
                Password = Environment.GetEnvironmentVariable("AMQP_PASSWORD"),
                ClientProvidedName = "MqNotifier",
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var basicProperties = channel.CreateBasicProperties();
            basicProperties.Type = Environment.GetEnvironmentVariable("AMQP_TYPE");
            basicProperties.AppId = Environment.GetEnvironmentVariable("AMQP_APPID");
            basicProperties.UserId = Environment.GetEnvironmentVariable("AMQP_USERNAME");
            
            var streamReader = new StreamReader(Console.OpenStandardInput());

            channel.BasicPublish(
                Environment.GetEnvironmentVariable("AMQP_EXCHANGE"),
                string.Empty,
                basicProperties,
                Encoding.UTF8.GetBytes(streamReader.ReadLine()));
            channel.Close();
            connection.Close();
        }
    }
}