using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MiniBite.Api.Integration
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private readonly IConfiguration _config;

        public AzureServiceBusMessageBus(IConfiguration config)
        {
            _config = config;
        }

        public async Task PublishMessage(IntegrationBaseMessage message, string topicName)
        {
            var connectionString = _config["AzureServiceBusConnectionString"].ToString();

            ISenderClient topicClient = new TopicClient(connectionString, topicName);
            var jsonMessage = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await topicClient.SendAsync(serviceBusMessage);
            Console.WriteLine(($"Sent message to {topicClient.Path}"));
            await topicClient.CloseAsync();
        }
    }
}
