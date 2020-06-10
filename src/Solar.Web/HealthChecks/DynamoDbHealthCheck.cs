using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Solar.Web.HealthChecks
{
    public class DynamoDbHealthCheck : IHealthCheck
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public DynamoDbHealthCheck(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _dynamoDb.DescribeTableAsync("Planets", cancellationToken);
            var isActive = response.Table.TableStatus == TableStatus.ACTIVE;

            return isActive
                ? HealthCheckResult.Healthy("DynamoDb is available")
                : HealthCheckResult.Unhealthy("DynamoDb is unavailable");
        }
    }
}
