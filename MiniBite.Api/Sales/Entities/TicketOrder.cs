using System;

namespace MiniBite.Api.Sales.Entities
{
    public class TicketOrder
    {
        public Guid TicketId { get; set; }
        public Guid OrderId { get; set; }
    }
}
