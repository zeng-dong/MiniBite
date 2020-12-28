namespace MiniBite.Api.Sales.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        void Start();
        void Stop();
    }
}
