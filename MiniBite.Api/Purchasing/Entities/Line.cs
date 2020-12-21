using System;

namespace MiniBite.Api.Purchasing.Entities
{
    public class Line
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }
        public Guid ItemId { get; set; }

        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Number { get; set; }
        public string LotNumber { get; set; }

        public Item Item { get; set; }
    }
}
