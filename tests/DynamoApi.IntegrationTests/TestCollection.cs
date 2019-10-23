using Xunit;

namespace DynamoApi.IntegrationTests
{
    [CollectionDefinition("Test Collection", DisableParallelization = true)]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}