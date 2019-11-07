using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynamoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoApi.Controllers
{
    [ApiController]
    [Route("/planets")]
    public class PlanetsController : ControllerBase
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public PlanetsController(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        [HttpGet("init")]
        public async Task<IActionResult> Initialise()
        {
            var context = new DynamoDBContext(_dynamoClient, new DynamoDBContextConfig { SkipVersionCheck = true });
            var write = context.CreateBatchWrite<Planet>();

            write.AddPutItems(new List<Planet>
                {
                    new Planet { Universe = "Milky Way", Name = "Mercury", Moons = new List<Moon>() },
                    new Planet { Universe = "Milky Way", Name = "Venus", Moons = new List<Moon>() },
                    new Planet { Universe = "Milky Way", Name = "Earth", Moons = new List<Moon>
                    {
                        new Moon { Id = 1, Name = "Moon" }
                    }},
                    new Planet { Universe = "Milky Way", Name = "Mars", Moons = new List<Moon>
                    {
                        new Moon { Id = 1, Name = "Phobos" },
                        new Moon { Id = 2, Name = "Deimos" }
                    }},
                    new Planet { Universe = "Milky Way", Name = "Jupiter", Moons = new List<Moon>
                    {
                        new Moon { Id = 1, Name = "Io" },
                        new Moon { Id = 2, Name = "Europa" },
                        new Moon { Id = 3, Name = "Ganymede" },
                        new Moon { Id = 4, Name = "Callisto" }
                    }}
                }
            );
            
            await write.ExecuteAsync();
            return new OkResult();
        }

        [HttpGet]
        public async Task<List<Planet>> Get_All()
        {
            var context = new DynamoDBContext(_dynamoClient);
            var planets = await context.ScanAsync<Planet>(new List<ScanCondition>()).GetRemainingAsync();
            return planets;
        }

        [HttpGet("{universe}")]
        public async Task<List<Planet>> GetForUniverse(string universe)
        {
            var context = new DynamoDBContext(_dynamoClient);
            var planets = await context.QueryAsync<Planet>(universe).GetRemainingAsync();
            return planets;
        }
        
        [HttpGet("{universe}/{planet}")]
        public async Task<List<Planet>> GetForUniverseAndPlanet(string universe, string planet)
        {
            var context = new DynamoDBContext(_dynamoClient);
            var planets = await context.QueryAsync<Planet>(universe, QueryOperator.Equal, new []{planet}).GetRemainingAsync();
            return planets;
        }

    }
}