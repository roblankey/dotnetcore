using System;
using DynamoApi.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DynamoApi.UnitTests.Models
{
    public class PlanetTest
    {
        private readonly Planet _planet;

        public PlanetTest()
        {
            _planet = new Planet
            {
                Id = new Random().Next(100),
                Version = 1,
                AlienScore = 3,
                Moons = "{ \"fry\": { \"atmosphere\": \"none\" }, \"leela\": { \"sky\": \"purple\" }, " +
                        "\"bender\": { \"liquid\": \"ethanol\" } }"
            };
        }

        [Fact]
        public void Planet_Model_Test()
        {
            Assert.Equal(1, _planet.Version);
            Assert.Equal(3, _planet.AlienScore);
            Assert.Equal("ethanol", JObject.Parse(_planet.Moons)["bender"]["liquid"]);
        }
    }
}