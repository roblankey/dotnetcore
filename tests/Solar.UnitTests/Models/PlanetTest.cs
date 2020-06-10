using Solar.Core.Entities;
using Xunit;

namespace Solar.UnitTests.Models
{
    public class PlanetTest
    {
        private readonly Planet _planet;

        public PlanetTest()
        {
            _planet = new Planet {
                Universe = "Milky Way", Name = "Jupiter"
            };
        }

        [Fact]
        public void Planet_Model_Test()
        {
            Assert.Equal("Milky Way", _planet.Universe);
            Assert.Equal("Jupiter", _planet.Name);
        }
    }
}