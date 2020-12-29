using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiniBite.Api.Inventory.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IBusControl ServiceBus { get; set; }

        public static IApplicationBuilder UseMassTransitInMemoryTransport(this IApplicationBuilder app)
        {
            ServiceBus = app.ApplicationServices.GetService<IBusControl>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            ServiceBus.StartAsync();
        }

        private static void OnStopping()
        {
            ServiceBus.StopAsync();
        }
    }
}
