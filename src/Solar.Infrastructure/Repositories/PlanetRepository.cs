using Amazon.DynamoDBv2;
using Solar.Core.Entities;

namespace Solar.Infrastructure.Repositories
{
    public class PlanetRepository : DynamoRepository<Planet>
    {
        private const string TableName = "Planets";

        public PlanetRepository(IAmazonDynamoDB dynamoDb) : base(dynamoDb, TableName)
        {
        }
    }
}
