using AutoMapper;
using MiniBite.Api.Purchasing.ApiModels;
using MiniBite.Api.Purchasing.Entities;
using MiniBite.Api.Purchasing.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiniBite.Api.Purchasing.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _client;
        readonly IMapper _mapper;
        public InventoryService(HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            //curl -X GET "https://localhost:5001/api/v1/inventory/products" -H "accept: */*"

            var response = await _client.GetAsync("api/v1/inventory/products");
            var dtos = await response.ReadContentAs<IEnumerable<InventoryProductDto>>();

            var items = _mapper.Map<IEnumerable<InventoryProductDto>, IEnumerable<Item>>(dtos);

            return items;
        }
    }
}
