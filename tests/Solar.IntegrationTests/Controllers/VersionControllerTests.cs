using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Solar.Web;
using Xunit;

namespace Solar.IntegrationTests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class VersionControllerTests : IClassFixture<TestAppFactory<Startup>>
    {
        private readonly TestAppFactory<Startup> _factory;

        public VersionControllerTests(TestAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Can_Get_Version()
        {
            var response = await _factory.CreateClient().GetAsync("/version");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = response.Content.ReadAsStringAsync().Result;
            Assert.NotNull(JObject.Parse(body)["version"]);
        }
    }
}
