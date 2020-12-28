using System.Threading.Tasks;

namespace MiniBite.Api.Integration
{
    public interface IMessageBus
    {
        Task PublishMessage(IntegrationBaseMessage message, string topicsName);
    }
}
