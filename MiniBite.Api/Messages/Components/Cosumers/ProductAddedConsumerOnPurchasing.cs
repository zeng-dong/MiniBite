using MassTransit;
using Microsoft.Extensions.Logging;
using MiniBite.Api.Messages.Contracts;
using System.Threading.Tasks;

namespace MiniBite.Api.Messages.Components.Cosumers
{
    public class ProductAddedConsumerOnPurchasing : IConsumer<IProductAdded>
    {
        private readonly ILogger<ProductAddedConsumerOnPurchasing> _logger;

        public ProductAddedConsumerOnPurchasing(ILogger<ProductAddedConsumerOnPurchasing> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IProductAdded> context)
        {
            _logger.Log(LogLevel.Debug, $"ProductAdded with code: {context.Message.Code} received by ProductAddedConsumerOnPurchasing");

            _logger.Log(LogLevel.Debug, $"Taking care of ProductAdded on the Purchasing side");

            await context.RespondAsync<ProductAddedReceived>(new ProductAddedReceived
            {
                OrderId = context.Message.Id,
                Timestamp = InVar.Timestamp,
                Message = context.Message.CategoryName
            });
        }
    }
}
