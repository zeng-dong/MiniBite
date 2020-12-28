using System;

namespace MiniBite.Api.Sales.Entities
{
    public class TicketCancellation
    {
        public Guid TicketId { get; set; }
        public Guid CancellationId { get; set; }
    }
}
