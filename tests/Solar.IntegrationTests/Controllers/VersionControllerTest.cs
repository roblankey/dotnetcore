using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Solar.IntegrationTests.Controllers
{
    [Collection("Test Collection")]
    public class VersionControllerTest
    {
        private readonly HttpClient _client;

        public VersionControllerTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Can_Get_Version()
        {
            var response = await _client.GetAsync("/version");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = response.Content.ReadAsStringAsync().Result;
            Assert.NotNull(JObject.Parse(body)["version"]);
        }
    }
}