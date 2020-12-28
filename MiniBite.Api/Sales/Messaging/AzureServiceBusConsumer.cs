using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using MiniBite.Api.Integration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBite.Api.Sales.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        private readonly string subscriptionName = "ticket-subscriber-01";
        private readonly string prodcutCreatedMessageTopic = "ticket-orders";
        private readonly IReceiverClient productCreatedMessageReceiverClient;

        public AzureServiceBusConsumer(IConfiguration config, IMessageBus messageBus)
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
            Console.WriteLine("Sales AzureServiceBusConsumer received: " + body);
        }

        private Task OnServiceBusException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs);

            return Task.CompletedTask;
        }


        public void Stop()
        {
            //throw new NotImplementedException();
        }
    }
}
