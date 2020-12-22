using MassTransit;
using Microsoft.Extensions.Logging;
using MiniBite.Api.Messages.Contracts;
using System.Threading.Tasks;

namespace MiniBite.Api.Messages.Components.Cosumers
{
    public class ProductAddedConsumerOnSales : IConsumer<IProductAdded>
    {
        private readonly ILogger<ProductAddedConsumerOnSales> _logger;

        public ProductAddedConsumerOnSales(ILogger<ProductAddedConsumerOnSales> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<IProductAdded> context)
        {
            _logger.Log(LogLevel.Debug, $"ProductAdded with code: {context.Message.Code} received by ProductAddedConsumerOnSales");

            _logger.Log(LogLevel.Debug, $"Taking care of ProductAdded on the Sales side");

            return Task.CompletedTask;
        }
    }
}
