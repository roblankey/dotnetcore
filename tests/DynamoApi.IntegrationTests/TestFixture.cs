using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using Serilog;

namespace DynamoApi.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        public readonly TestServer Server;
        public readonly HttpClient Client;

        public TestFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Test")
                .UseSerilog());
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        public static T DeserializeResponse<T>(HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(json).ToObject<T>();
        }
        
        public static List<T> DeserializeResponseList<T>(HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            return JArray.Parse(json).ToObject<List<T>>();
        }

        public T GetService<T>()
        {
            return (T)Server.Host.Services.GetService(typeof(T));
        }
    }
}
