using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using ServiceStack.Aws.DynamoDb;

namespace Solar.Web.HealthChecks
{
    public class DynamoDbHealthCheck : IHealthCheck
    {
        private readonly IPocoDynamo _dynamoDb;
        private readonly ILogger _logger;

        public DynamoDbHealthCheck(IPocoDynamo dynamoDb, ILoggerFactory loggerFactory)
        {
            _dynamoDb = dynamoDb;
            _logger = loggerFactory.CreateLogger<DynamoDbHealthCheck>();
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var response = await _dynamoDb.DynamoDb.DescribeTableAsync("Planets", cancellationToken);
                var isActive = response.Table.TableStatus == TableStatus.ACTIVE;

                return isActive
                    ? HealthCheckResult.Healthy("DynamoDb is available")
                    : HealthCheckResult.Unhealthy("DynamoDb is unavailable");
            }
            catch (Exception e)
            {
                _logger.LogCritical("Could not connect to DynamoDb!");
                return HealthCheckResult.Unhealthy(e.Message);
            }
        }
    }
}
