using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Integration;
using MiniBite.Api.Inventory.DataAccess;
using MiniBite.Api.Inventory.Messages;
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
        readonly IMessageBus _messageBus;

        public ProductsController(InventoryDbContext context, IRequestClient<IProductAdded> productAddedClient, IMessageBus messageBus)
        {
            _context = context;
            _productAddedClient = productAddedClient;
            _messageBus = messageBus;
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

        [HttpPost("publish-to-aztopic")]
        public async Task<ActionResult> CreateAndPublishMessageToAzureServiceBus(string code, string description, CancellationToken cancellationToken)
        {
            try
            {
                var msg = new ProductCreatedMessage()
                {
                    Id = Guid.NewGuid(),
                    CreationDateTime = DateTime.UtcNow,
                    ProductCode = code,
                    ProductDescription = description,
                    ProductId = new Guid("CDD8341C-BBA6-4049-9B14-D400D3C068D1")
                };

                await _messageBus.PublishMessage(msg, "ticket-orders");

                return Ok(msg);
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
