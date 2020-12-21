using System;
using System.Collections.Generic;

namespace MiniBite.Api.Purchasing.Entities
{
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            Lines = new List<Line>();
        }
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Timestamp { get; set; }

        public IList<Line> Lines { get; set; }
    }
}
