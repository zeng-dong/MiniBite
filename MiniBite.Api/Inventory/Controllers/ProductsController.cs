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
        private IBus _bus;

        //public ProductsController(InventoryDbContext context, IRequestClient<IProductAdded> productAddedClient, IMessageBus messageBus)
        //{
        //    _context = context;
        //    _productAddedClient = productAddedClient;
        //    _messageBus = messageBus;
        //}

        public ProductsController(InventoryDbContext context, IMessageBus messageBus, IBus bus)
        {
            _context = context;
            _messageBus = messageBus;
            _bus = bus;
        }

        [HttpGet("products")]
        public async Task<ActionResult> GetAll()
        {
            var all = await _context.Proudcts.ToListAsync();
            return Ok(all);
        }

        [HttpPost("post-to-inmemory")]
        public async Task<ActionResult> Create(string code, string description, CancellationToken cancellationToken)
        {
            /* output in console: working with me starting IBusControl in Startup
            dbug: MassTransit[0]
      Create send transport: loopback://localhost/urn:message:MiniBite.Api.Messages.Contracts:OrderMessage
dbug: MassTransit.SendTransport[0]
      SEND loopback://localhost/urn:message:MiniBite.Api.Messages.Contracts:OrderMessage c6ed0000-5d40-0015-c95a-08d8aba22af9 MiniBite.Api.Messages.Contracts.OrderMessage
Received: its text is testing mess, its order ID is 124
MessageConsumer-2: OrderId: 124 text: testing mess
Taking care of OrderMessage by Cosumer-2
MessageConsumer-1: OrderId: 124 text: testing mess
Taking care of OrderMessage by Consumer-1
             *
             */

            try
            {
                var msg = new OrderMessage()
                {
                    OrderId = 124,
                    Text = "testing mess"
                };

                await _bus.Publish(msg);

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

        //[HttpPost("masstransit-mediator")]
        //public async Task<ActionResult> Create(string code, string description, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var response = await _productAddedClient.GetResponse<ProductAddedReceived>(new
        //        {
        //            Id = Guid.NewGuid(),
        //            Code = code,
        //            Description = description,
        //            Price = 2.55M,
        //            CategoryName = "some category"
        //        }, cancellationToken);
        //
        //        return Ok(response.Message);
        //    }
        //    catch (RequestTimeoutException)
        //    {
        //        return StatusCode((int)HttpStatusCode.RequestTimeout);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode((int)HttpStatusCode.RequestTimeout);
        //    }
        //}

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
