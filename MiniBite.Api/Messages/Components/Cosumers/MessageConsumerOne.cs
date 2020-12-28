using MassTransit;
using MiniBite.Api.Messages.Contracts;
using System;
using System.Threading.Tasks;

namespace MiniBite.Api.Messages.Components.Cosumers
{
    public class MessageConsumerOne : IConsumer<OrderMessage>
    {
        public Task Consume(ConsumeContext<OrderMessage> context)
        {
            //_logger.Log(LogLevel.Debug, $"MessageConsumer-1: OrderId: {context.Message.OrderId} text: {context.Message.Text}");
            //_logger.Log(LogLevel.Debug, $"Taking care of OrderMessage by Cosumer-1");
            Console.Out.WriteLineAsync($"MessageConsumer-1: OrderId: {context.Message.OrderId} text: {context.Message.Text}");
            Console.Out.WriteLineAsync($"Taking care of OrderMessage by Cosumer-1");

            return Task.CompletedTask;
        }
    }
}
