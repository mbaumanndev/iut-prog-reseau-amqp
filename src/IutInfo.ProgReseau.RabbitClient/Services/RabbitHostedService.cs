using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;
using IutInfo.ProgReseau.RabbitClient.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IutInfo.ProgReseau.RabbitClient.Services
{
    public sealed class RabbitHostedService : BackgroundService
    {
        private ILogger m_Logger;
        private IConnection m_Connection;
        private IModel m_Channel;
        private IHubContext<RabbitEventHub> m_HubContext;

        public RabbitHostedService(ILogger<RabbitHostedService> p_Logger, IHubContext<RabbitEventHub> p_HubContext)
        {
            m_Logger = p_Logger;
            m_HubContext = p_HubContext;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "rabbit" };

            // create connection
            m_Connection = factory.CreateConnection();

            // create channel
            m_Channel = m_Connection.CreateModel();

            m_Channel.ExchangeDeclare("client.exchange", ExchangeType.Topic, true);
            m_Channel.QueueDeclare("client.queue.log", false, false, false, null);
            m_Channel.QueueBind("client.queue.log", "client.exchange", "client.queue.*", null);
            m_Channel.BasicQos(0, 1, false);

            m_Connection.ConnectionShutdown += RabbitMqMConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(m_Channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                // handle the received message
                HandleMessage(content);
                m_Channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            m_Channel.BasicConsume("client.queue.log", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string content)
        {
            m_Logger.LogInformation($"consumer received {content}");

            m_HubContext.Clients.All.SendAsync("Rabbit", content);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMqMConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            m_Channel.Close();
            m_Connection.Close();
            base.Dispose();
        }
    }
}