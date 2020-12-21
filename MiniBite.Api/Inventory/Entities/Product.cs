using System;

namespace MiniBite.Api.Inventory.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
