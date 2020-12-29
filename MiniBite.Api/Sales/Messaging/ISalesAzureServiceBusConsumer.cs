namespace MiniBite.Api.Sales.Messaging
{
    public interface ISalesAzureServiceBusConsumer
    {
        void Start();
        void Stop();
    }
}
