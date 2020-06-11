using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Solar.Web.Controllers
{
    [ApiController]
    [Route("/version")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetVersion()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            return Ok(new {Version = version});
        }
    }
}
