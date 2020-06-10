using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solar.Core.Entities;
using Solar.Core.Interfaces;

namespace Solar.Web.Controllers
{
    [ApiController]
    [Route("/planets")]
    public class PlanetsController : ControllerBase
    {
        private readonly IAsyncRepository<Planet> _planetAsyncRepository;

        public PlanetsController(IAsyncRepository<Planet> planetAsyncRepository)
        {
            _planetAsyncRepository = planetAsyncRepository;
        }

        [HttpGet]
        public async Task<IReadOnlyList<Planet>> Get_All()
        {
            return await _planetAsyncRepository.ListAllAsync();
        }
    }
}
