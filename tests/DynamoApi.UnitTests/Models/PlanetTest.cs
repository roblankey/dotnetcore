using System.Collections.Generic;
using DynamoApi.Models;
using Xunit;

namespace DynamoApi.UnitTests.Models
{
    public class PlanetTest
    {
        private readonly Planet _planet;

        public PlanetTest()
        {
            _planet = new Planet {
                Universe = "Milky Way", Name = "Jupiter", Moons = new List<Moon> 
                {
                    new Moon { Id = 1, Name = "Io" },
                    new Moon { Id = 2, Name = "Europa" },
                    new Moon { Id = 3, Name = "Ganymede" },
                    new Moon { Id = 4, Name = "Callisto" }
                }
            };
        }

        [Fact]
        public void Planet_Model_Test()
        {
            Assert.Equal("Milky Way", _planet.Universe);
            Assert.Equal("Jupiter", _planet.Name);
            Assert.Equal(4, _planet.Moons.Count);
        }
    }
}