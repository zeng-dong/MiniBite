using System;

namespace MiniBite.ConsoleSubscriber.Contracts
{
    public class TicketCancellation
    {
        public Guid TicketId { get; set; }
        public Guid CancellationId { get; set; }
    }
}
