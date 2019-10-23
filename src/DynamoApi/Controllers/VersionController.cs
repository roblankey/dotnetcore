using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace DynamoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionController
    {
        [HttpGet]
        public IActionResult GetVersion()
        {
            
            // ReSharper disable once AssignNullToNotNullAttribute
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            
            var content = new
            {
                Version = version
            };
            return new OkObjectResult(content);
        }
    }
}