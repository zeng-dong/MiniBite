using System;

namespace MiniBite.ConsoleSubscriber.Contracts
{
    public class TicketOrder
    {
        public Guid TicketId { get; set; }
        public Guid OrderId { get; set; }
    }
}
