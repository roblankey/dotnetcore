using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Solar.Web.HealthChecks;
using Xunit;

namespace Solar.UnitTests.HealthChecks
{
    [ExcludeFromCodeCoverage]
    public class AliveHealthCheckTests
    {
        [Fact]
        public async Task Alive_Check_Returns_Healthy()
        {
            var check = new AliveHealthCheck();
            var health = await check.CheckHealthAsync(new HealthCheckContext());
            Assert.Equal(HealthStatus.Healthy, health.Status);
        }
    }
}
