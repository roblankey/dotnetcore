using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Solar.Core.Entities;
using Solar.Core.Interfaces;
using Solar.IntegrationTests.Repositories;

namespace Solar.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class TestAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IAsyncRepository<Planet>, FakePlanetRepository>();
            });
        }

        public T DeserializeResponse<T>(HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T GetService<T>()
        {
            using var scope = Server.Services.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }
    }
}
