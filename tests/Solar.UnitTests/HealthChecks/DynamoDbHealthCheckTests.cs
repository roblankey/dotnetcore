using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ServiceStack.Aws.DynamoDb;
using Solar.Web.HealthChecks;
using Xunit;

namespace Solar.UnitTests.HealthChecks
{
    [ExcludeFromCodeCoverage]
    public class DynamoDbHealthCheckTests
    {
        private readonly IAmazonDynamoDB _awsDynamo = Substitute.For<IAmazonDynamoDB>();
        private readonly IPocoDynamo _pocoDynamo = Substitute.For<IPocoDynamo>();

        public DynamoDbHealthCheckTests()
        {
            _pocoDynamo.DynamoDb.Returns(_awsDynamo);
        }

        [Fact]
        public async Task Healthy_Dynamo_Connection_Returns_Healthy()
        {
            _awsDynamo.DescribeTableAsync(Arg.Any<string>()).Returns(new DescribeTableResponse
            {
                Table = new TableDescription
                {
                    TableStatus = TableStatus.ACTIVE
                }
            });
            
            var check = new DynamoDbHealthCheck(_pocoDynamo, new NullLoggerFactory());
            var health = await check.CheckHealthAsync(new HealthCheckContext());
            Assert.Equal(HealthStatus.Healthy, health.Status);
        }
        
        [Fact]
        public async Task Inactive_Table_Dynamo_Returns_Unhealthy()
        {
            _awsDynamo.DescribeTableAsync(Arg.Any<string>()).Returns(new DescribeTableResponse
            {
                Table = new TableDescription
                {
                    TableStatus = TableStatus.CREATING
                }
            });
            
            var check = new DynamoDbHealthCheck(_pocoDynamo, new NullLoggerFactory());
            var health = await check.CheckHealthAsync(new HealthCheckContext());
            Assert.Equal(HealthStatus.Unhealthy, health.Status);
        }
        
        [Fact]
        public async Task Dynamo_Connection_Exception_Returns_Unhealthy()
        {
            _awsDynamo.DescribeTableAsync(Arg.Any<string>()).Throws(new Exception("No Table for You!"));
            
            var check = new DynamoDbHealthCheck(_pocoDynamo, new NullLoggerFactory());
            var health = await check.CheckHealthAsync(new HealthCheckContext());
            Assert.Equal(HealthStatus.Unhealthy, health.Status);
        }
    }
}
