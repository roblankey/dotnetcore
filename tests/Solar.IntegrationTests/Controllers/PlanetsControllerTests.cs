using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Solar.Core.Entities;
using Solar.Core.Interfaces;
using Solar.Web;
using Xunit;

namespace Solar.IntegrationTests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class PlanetsControllerTests : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly TestAppFactory<Startup> _factory;
        private readonly Faker _faker = new Faker();

        public PlanetsControllerTests(TestAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Can_Get_Planets()
        {
            var response = await _factory.CreateClient().GetAsync("/planets");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actual = _factory.DeserializeResponse<List<Planet>>(response);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
        }
        
        [Fact]
        public async Task Can_Get_Planets_For_Universe()
        {
            var response = await _factory.CreateClient().GetAsync("/planets/Milky%20Way");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actual = _factory.DeserializeResponse<List<Planet>>(response);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.True(actual.All(p => p.Universe == "Milky Way"));
        }
        
        [Fact]
        public async Task Can_Get_Planet_For_Universe()
        {
            var response = await _factory.CreateClient().GetAsync("/planets/Milky%20Way/Mars");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actual = _factory.DeserializeResponse<Planet>(response);
            Assert.NotNull(actual);
            Assert.Equal("Milky Way", actual.Universe);
            Assert.Equal("Mars", actual.Name);
        }
        
        [Fact]
        public async Task Can_Post_Planet()
        {
            var planet = new Planet
            {
                Id = new Random().Next(),
                Name = _faker.Internet.DomainWord(),
                Universe = _faker.Internet.DomainName()
            };
            
            var response = await _factory.CreateClient().PostAsync(
                "/planets",
                new StringContent(JsonConvert.SerializeObject(planet), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actual = _factory.DeserializeResponse<Planet>(response);
            Assert.NotNull(actual);
            Assert.Equal(planet.Id, actual.Id);
            Assert.Equal(planet.Name, actual.Name);
            Assert.Equal(planet.Universe, actual.Universe);

            var repository = _factory.GetService<IAsyncRepository<Planet>>();
            Assert.NotNull(await repository.GetByIdAsync(planet.Id));
        }
        
        [Fact]
        public async Task Can_Patch_Planet()
        {
            var patches = new JsonPatchDocument<Planet>();
            patches.Replace(p => p.Name, "Pluto");
            
            var response = await _factory.CreateClient().PatchAsync(
                "/planets/Milky%20Way/Earth",
                new StringContent(JsonConvert.SerializeObject(patches), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var actual = _factory.DeserializeResponse<Planet>(response);
            Assert.NotNull(actual);
            Assert.Equal("Milky Way", actual.Universe);
            Assert.Equal("Pluto", actual.Name);

            var stored = await _factory.GetService<IAsyncRepository<Planet>>().GetByIdAsync(actual.Id);
            Assert.Equal("Pluto", stored.Name);
        }
        
        [Fact]
        public async Task Can_Delete_Planet()
        {
            var planet = new Planet
            {
                Id = new Random().Next(),
                Name = _faker.Internet.DomainWord(),
                Universe = _faker.Internet.DomainName()
            };
            await _factory.GetService<IAsyncRepository<Planet>>().AddAsync(planet);

            var response = await _factory.CreateClient().DeleteAsync($"/planets/{planet.Universe}/{planet.Name}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            var stored = await _factory.GetService<IAsyncRepository<Planet>>().GetByIdAsync(planet.Id);
            Assert.Null(stored);
        }
    }
}
