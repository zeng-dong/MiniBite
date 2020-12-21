using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiniBite.Api.Inventory.DataAccess;
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
            services.AddDbContext<InventoryDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "minibite-inventory");
            });
            services.AddDbContext<PurchasingDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "minibite-purchasing");
            });


            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Multiple APIs",
                    Description = "A simple API to .... from ...",
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
        }
    }
}
