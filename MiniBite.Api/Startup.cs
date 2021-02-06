using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiniBite.Api.Inventory.DataAccess;
using MiniBite.Api.Inventory.Extensions;
using MiniBite.Api.Messages.Components.Consumers;
using MiniBite.Api.Messages.Contracts;
using MiniBite.Api.Purchasing.DataAccess;
using MiniBite.Api.Purchasing.Services;
using System;
using System.Linq;

namespace MiniBite.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var asbcs = Configuration["AzureServiceBusConnectionString"].ToString();
            string ticketOrdersTopic = "ticket-orders";

            //var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
            //{
            //    busFactoryConfig.Message<TicketOrder>(configTopology => { configTopology.SetEntityName(ticketOrdersTopic); });
            //
            //    busFactoryConfig.Host(asbcs, hostConfig =>
            //    {
            //        hostConfig.TransportType = Microsoft.Azure.ServiceBus.TransportType.AmqpWebSockets;
            //    });
            //});
            //services.AddMassTransit(config => config.AddBus(provider => azureServiceBus));
            //services.AddSingleton<IPublishEndpoint>(azureServiceBus);
            //services.AddSingleton<ISendEndpointProvider>(azureServiceBus);
            //services.AddSingleton<IBus>(azureServiceBus);            // optional

            // I try to use inmemory bus here
            var inMemorybus = Bus.Factory.CreateUsingInMemory(sbc =>
            {
                sbc.ReceiveEndpoint("order_queue", ep =>
                {
                    ep.Handler<OrderMessage>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: its text is {context.Message.Text}, its order ID is {context.Message.OrderId}");
                    });

                    ep.Consumer<MessageConsumerOne>();
                    ep.Consumer<MessageConsumerTwo>();
                });
            });
            services.AddMassTransit(config => config.AddBus(provider => inMemorybus));
            services.AddSingleton<IBus>(inMemorybus);

            services.AddMediator(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ProductAddedConsumerOnPurchasing>();

                cfg.AddRequestClient<IProductAdded>();
            });

            services.AddDbContext<InventoryDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "minibite-inventory");
            });
            services.AddDbContext<PurchasingDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "minibite-purchasing");
            });


            services.AddControllers();

            var secret = Configuration["SuperSecret"].ToString();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Multiple APIs",
                    Description = secret,
                    Contact = new OpenApiContact
                    {
                        Name = "Multiple APIs",
                        Email = "someone@somewhere.net",
                        Url = new Uri("https://www.someone.org")
                    }
                });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });

            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                });

            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient<IInventoryService, InventoryService>(c =>
                c.BaseAddress = new Uri(Configuration["ApiConfigs:Inventory:Uri"]));
            services.AddSingleton<PurchasingDbInitializer>();

            // I use this bus to post
            //services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

            // I use these consumers to consume
            //services.AddSingleton<ISalesAzureServiceBusConsumer, SalesAzureServiceBusConsumer>();
            //services.AddSingleton<IPurchasingAzureServiceBusConsumer, PurchasingAzureServiceBusConsumer>();
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            InventoryDbContext inventoryDbContext
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!inventoryDbContext.Proudcts.Any())
            {
                InventoryDbInitializer.Seed(inventoryDbContext);
            }
            // cannot do purchasing initialization here utilizing httpClient because the service is not up

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // note: need to start the bus
            //app.UseSalesAzureServiceBusConsumer();
            //app.UsePurchasingAzureServiceBusConsumer();

            app.UseMassTransitInMemoryTransport();
        }
    }
}
