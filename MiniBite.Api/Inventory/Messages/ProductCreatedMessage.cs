using MiniBite.Api.Integration;
using System;

namespace MiniBite.Api.Inventory.Messages
{
    public class ProductCreatedMessage : IntegrationBaseMessage
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }

    }
}
