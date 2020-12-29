using System.Threading.Tasks;

namespace MiniBite.Api.Integration
{
    public interface IMessageBusAnother
    {
        Task PublishMessage(IntegrationBaseMessage message);
    }
}
