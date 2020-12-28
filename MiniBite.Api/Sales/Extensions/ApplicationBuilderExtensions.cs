using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniBite.Api.Sales.Messaging;

namespace MiniBite.Api.Sales.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseSalesAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStopping()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
