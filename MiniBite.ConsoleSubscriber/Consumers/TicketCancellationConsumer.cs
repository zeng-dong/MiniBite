using MassTransit;
using MiniBite.ConsoleSubscriber.Contracts;
using Serilog;
using System.Threading.Tasks;

namespace MiniBite.ConsoleSubscriber.Consumers
{
    public class TicketCancellationConsumer : IConsumer<TicketCancellation>
    {
        public Task Consume(ConsumeContext<TicketCancellation> context)
        {
            Log.Information($"Ticket Dequeued- TicketId: {context.Message.TicketId} CancellationId: {context.Message.CancellationId}");

            return Task.CompletedTask;
        }
    }
}
