using System;
using System.Threading.Tasks;

namespace MiniBite.Api.Integration
{
    public class MassTransitInMemoryBus : IMessageBusAnother
    {
        public Task PublishMessage(IntegrationBaseMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
