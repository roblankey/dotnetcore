using System;
using System.Linq;
using System.Net.Mime;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Solar.Core.Entities;
using Solar.Core.Interfaces;
using Solar.Infrastructure.Repositories;
using Solar.Web.HealthChecks;

namespace Solar.Web
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                status = report.Status.ToString(),
                                errors = report.Entries.Select(e => new
                                {
                                    key = e.Key,
                                    value = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API v1"); });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHealthChecks()
                .AddCheck<AliveHealthCheck>("alive_health_check")
                .AddCheck<DynamoDbHealthCheck>("dynamo_health_check");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Example API", Version = "v1"});
            });

            var dynamoDbConfig = Program.Configuration.GetSection("DynamoDb");
            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")
                };
                return new AmazonDynamoDBClient(clientConfig);
            });

            services.AddScoped<IAsyncRepository<Planet>, PlanetRepository>();
        }
    }
}
