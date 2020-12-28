using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniBite.Api.Sales.Entities;
using System;
using System.Threading.Tasks;

namespace MiniBite.Api.Sales.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/sales")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IConfiguration _configuration;

        public TicketsController(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
            _configuration = configuration;
        }

        [HttpPost("place-order")]
        public async Task<ActionResult> Create()
        {
            var order = new TicketOrder { TicketId = Guid.NewGuid(), OrderId = Guid.NewGuid() };

            await _publishEndpoint.Publish<TicketOrder>(order);

            return Ok(order);
        }

        [HttpPost("cancel-order")]
        public async Task<ActionResult> Cancel()
        {
            var queueName = _configuration["TicketCancellationQueueName"].ToString();
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(queueName));

            var cancel = new TicketCancellation { TicketId = Guid.NewGuid(), CancellationId = Guid.NewGuid() };

            await sendEndpoint.Send<TicketCancellation>(cancel);

            return Ok(cancel);
        }
    }
}
