using MiniBite.Api.Purchasing.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBite.Api.Purchasing.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<Item>> GetItems();
    }
}
