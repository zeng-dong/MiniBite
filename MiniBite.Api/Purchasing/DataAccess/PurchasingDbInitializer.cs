using MiniBite.Api.Purchasing.Services;

namespace MiniBite.Api.Purchasing.DataAccess
{
    public class PurchasingDbInitializer
    {
        private IInventoryService _inventory;

        public PurchasingDbInitializer(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        public void Seed(PurchasingDbContext context)
        {
            var items = _inventory.GetItems().Result;
            context.Items.AddRange(items);

            //            
            //context.PurchaseOrders.AddRange(CreatePurchaseOrderList());

            context.SaveChanges();
        }


    }
}
