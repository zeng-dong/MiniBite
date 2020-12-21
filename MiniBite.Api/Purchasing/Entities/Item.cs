using System;

namespace MiniBite.Api.Purchasing.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal UnitCost { get; set; }
    }
}
