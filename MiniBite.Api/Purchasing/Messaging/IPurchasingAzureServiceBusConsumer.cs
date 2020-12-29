namespace MiniBite.Api.Purchasing.Messaging
{
    public interface IPurchasingAzureServiceBusConsumer
    {
        void Start();
        void Stop();
    }
}
