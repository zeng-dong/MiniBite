using MassTransit;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniBite.ConsoleSubscriber.Consumers;
using MiniBite.ConsoleSubscriber.Contracts;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace MiniBite.ConsoleSubscriber
{
    class ConsoleSubscriber
    {
        private static IServiceProvider _servicesProvider;
        private static string ConnectionString = String.Empty;

        public static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ConsoleSubscriber).Assembly)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton(config);
            _servicesProvider = services.BuildServiceProvider();
            ConnectionString = config["AzureServiceBusConnectionString"].ToString();

            var lc = new LoggerConfiguration().MinimumLevel.Information();
            Log.Logger = lc
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("MiniBite.ConsoleSubscriber", LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    "log.txt",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1)
                )
                .CreateLogger();

            await ConfigHostWithMassTransitAndRun();
        }

        private static async Task ConfigHostWithMassTransitAndRun()
        {
            Log.Debug("ConfigHostWithMassTransitAndRun() starting ...");
            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(config =>
                    {
                        config.AddConsumer<TicketPurchasedConsumer>();
                        config.AddConsumer<TicketCancellationConsumer>();
                        config.AddBus(ConfigureBus);
                    });

                    services.AddSingleton<IHostedService, MassTransitConsoleHostService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole();
                });

            Log.Debug("ConfigHostWithMassTransitAndRun() RunConsoleAsync ...");
            await builder.RunConsoleAsync();
        }

        static IBusControl ConfigureBus(IServiceProvider provider)
        {
            string ticketOrdersTopic = "ticket-orders";
            string subscriptionName = "ticket-subscriber-01";
            string queueName = "ticket-cancellation";

            var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
            {
                busFactoryConfig.Message<TicketOrder>(x => x.SetEntityName(ticketOrdersTopic));

                busFactoryConfig.Host(ConnectionString, hostConfig => hostConfig.TransportType = TransportType.AmqpWebSockets);

                busFactoryConfig.SubscriptionEndpoint<TicketOrder>(subscriptionName, configurator =>
                {
                    configurator.Consumer<TicketPurchasedConsumer>(provider);
                });

                // setup Azure queue consumer
                busFactoryConfig.ReceiveEndpoint(queueName, configurator =>
                {
                    configurator.Consumer<TicketCancellationConsumer>(provider);

                    // as this is a queue, no need to subscribe to topics, so set this to false.
                    // configurator.SubscribeMessageTopics = false;
                });
            });

            return azureServiceBus;
        }
    }
}
