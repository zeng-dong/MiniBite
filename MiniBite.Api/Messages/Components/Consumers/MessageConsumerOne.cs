using MassTransit;
using MiniBite.Api.Messages.Contracts;
using System;
using System.Threading.Tasks;

namespace MiniBite.Api.Messages.Components.Consumers
{
    public class MessageConsumerOne : IConsumer<OrderMessage>
    {
        public Task Consume(ConsumeContext<OrderMessage> context)
        {
            Console.Out.WriteLineAsync($"MessageConsumer-1: OrderId: {context.Message.OrderId} text: {context.Message.Text}");
            Console.Out.WriteLineAsync($"Taking care of OrderMessage by Consumer-1");

            return Task.CompletedTask;
        }
    }
}
