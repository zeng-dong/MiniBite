using MassTransit;
using MiniBite.ConsoleSubscriber.Contracts;
using Serilog;
using System.Threading.Tasks;

namespace MiniBite.ConsoleSubscriber.Consumers
{
    public class TicketPurchasedConsumer : IConsumer<TicketOrder>
    {
        public Task Consume(ConsumeContext<TicketOrder> context)
        {
            Log.Information($"Order processed: FlightId:{context.Message.TicketId} - OrderId:{context.Message.OrderId}");

            return Task.CompletedTask;
        }
    }
}
