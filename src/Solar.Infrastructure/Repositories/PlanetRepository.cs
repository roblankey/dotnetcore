using ServiceStack.Aws.DynamoDb;
using Solar.Core.Entities;

namespace Solar.Infrastructure.Repositories
{
    public class PlanetRepository : DynamoDbRepository<Planet>
    {
        public PlanetRepository(IPocoDynamo dynamoDb) : base(dynamoDb)
        {
        }
    }
}
