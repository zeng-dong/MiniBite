using System;

namespace MiniBite.Api.Messages.Contracts
{
    public class ProductAddedReceived
    {
        public Guid OrderId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }
}
