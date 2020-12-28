using MassTransit;
using MiniBite.Api.Messages.Contracts;
using System;
using System.Threading.Tasks;

namespace MiniBite.Api.Messages.Components.Cosumers
{
    public class MessageConsumerTwo : IConsumer<OrderMessage>
    {
        public Task Consume(ConsumeContext<OrderMessage> context)
        {
            Console.Out.WriteLineAsync($"MessageConsumer-2: OrderId: {context.Message.OrderId} text: {context.Message.Text}");
            Console.Out.WriteLineAsync($"Taking care of OrderMessage by Cosumer-2");

            return Task.CompletedTask;
        }
    }
}
