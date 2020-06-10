using Xunit;

namespace Solar.IntegrationTests
{
    [CollectionDefinition("Test Collection", DisableParallelization = true)]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}
