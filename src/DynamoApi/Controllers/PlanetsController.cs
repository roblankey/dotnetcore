using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetsController : ControllerBase
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public PlanetsController(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        [HttpGet]
        [Route("init")]
        public async Task<IActionResult> Initialise()
        {
            var context = new DynamoDBContext(_dynamoClient, new DynamoDBContextConfig { SkipVersionCheck = true });
            var write = context.CreateBatchWrite<Planet>();

            var random = new Random();
            write.AddPutItems(new List<Planet>
                {
                    new Planet { Id = random.Next(100), AlienScore = 0, Moons = "{ \"moon\": \"nasa\" }" },
                    new Planet { Id = random.Next(100), AlienScore = 3, Moons = "{ \"io\": \"none\", \"betel\": \"unlikely\" }" },
                    new Planet { Id = random.Next(100), AlienScore = 3, Moons = "{ \"hades\": \"unlikely (heat)\" }" },
                    new Planet { Id = random.Next(100), AlienScore = 7, Moons = "{ \"jupiter\": \"somewhat likely\" }" },
                    new Planet { Id = random.Next(100), AlienScore = 10, Moons = "{ \"ce-635\": \"likely\", \"ce-636\": \"very likely\" }" }
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

    }
}