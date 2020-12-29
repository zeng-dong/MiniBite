using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using MiniBite.Api.Integration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBite.Api.Purchasing.Messaging
{
    public class PurchasingAzureServiceBusConsumer : IPurchasingAzureServiceBusConsumer
    {

        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        private readonly string subscriptionName = "ticket-subscriber-02";
        private readonly string prodcutCreatedMessageTopic = "ticket-orders";
        private readonly IReceiverClient productCreatedMessageReceiverClient;

        public PurchasingAzureServiceBusConsumer(IConfiguration config, IMessageBus messageBus)
        {
            _configuration = config;
            _messageBus = messageBus;

            var serviceBusConnectionString = _configuration.GetValue<string>("AzureServiceBusConnectionString");

            productCreatedMessageReceiverClient = new SubscriptionClient(serviceBusConnectionString,
                prodcutCreatedMessageTopic, subscriptionName);
        }

        public void Start()
        {
            var messageHandlerOptions = new MessageHandlerOptions(OnServiceBusException) { MaxConcurrentCalls = 4 };

            productCreatedMessageReceiverClient.RegisterMessageHandler(OnProductCreatedMessageReceived, messageHandlerOptions);
        }

        private async Task OnProductCreatedMessageReceived(Message message, CancellationToken cancellationToken)
        {
            var body = Encoding.UTF8.GetString(message.Body);
            await Console.Out.WriteLineAsync("Purchasing AzureServiceBusConsumer received: " + body);
        }

        private Task OnServiceBusException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs);

            return Task.CompletedTask;
        }


        public void Stop()
        {
        }
    }
}
