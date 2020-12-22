using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Inventory.DataAccess;
using MiniBite.Api.Messages.Contracts;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MiniBite.Api.Inventory.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/inventory")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly InventoryDbContext _context;
        readonly IRequestClient<IProductAdded> _productAddedClient;

        public ProductsController(InventoryDbContext context, IRequestClient<IProductAdded> productAddedClient)
        {
            _context = context;
            _productAddedClient = productAddedClient;
        }

        [HttpGet("products")]
        public async Task<ActionResult> GetAll()
        {
            var all = await _context.Proudcts.ToListAsync();
            return Ok(all);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string code, string description, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _productAddedClient.GetResponse<ProductAddedReceived>(new
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    Price = 2.55M,
                    CategoryName = "some category"
                }, cancellationToken);

                return Ok(response.Message);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }

    }
}
