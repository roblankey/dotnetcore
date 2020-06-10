using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Solar.Core.Entities;
using Xunit;

namespace Solar.IntegrationTests.Controllers
{
    [Collection("Test Collection")]
    public class PlanetsControllerTest
    {
        private readonly HttpClient _client;

        public PlanetsControllerTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Can_Get_Planets()
        {
            var response = await _client.GetAsync("/planets");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actual = TestFixture.DeserializeResponseList<Planet>(response);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
        }
    }
}
