using System;

namespace MiniBite.Api.Messages.Contracts
{
    public class IProductAdded
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
