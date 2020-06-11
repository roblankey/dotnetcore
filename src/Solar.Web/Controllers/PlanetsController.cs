using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Solar.Core.Entities;
using Solar.Core.Interfaces;
using Solar.Core.Specifications;

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
        public async Task<ActionResult<IReadOnlyList<Planet>>> Get_All()
        {
            return Ok(await _planetAsyncRepository.ListAllAsync());
        }

        [HttpGet("{universe}")]
        public async Task<ActionResult<IReadOnlyList<Planet>>> Get_ForUniverse(string universe)
        {
            return Ok(await _planetAsyncRepository.ListAsync(new PlanetsSpecification(universe)));
        }

        [HttpGet("{universe}/{planet}")]
        public async Task<ActionResult<Planet>> Get_ForUniverseAndPlanet(string universe, string planet)
        {
            var results =  await _planetAsyncRepository.ListAsync(new PlanetsSpecification(universe, planet));
            var result = results.FirstOrDefault();

            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Planet>> Post_Planet(Planet planet)
        {
            return Ok(await _planetAsyncRepository.AddAsync(planet));
        }

        [HttpPatch("{universe}/{planet}")]
        public async Task<ActionResult<Planet>> Patch_Planet(string universe, string planet, JsonPatchDocument<Planet> patchDocument)
        {
            var results =  await _planetAsyncRepository.ListAsync(new PlanetsSpecification(universe, planet));
            var result = results.FirstOrDefault();

            if (result == null) return NotFound();
            patchDocument.ApplyTo(result);

            await _planetAsyncRepository.UpdateAsync(result);
            return Ok(result);
        }

        [HttpDelete("{universe}/{planet}")]
        public async Task<ActionResult> Delete_Planet(string universe, string planet)
        {
            var results =  await _planetAsyncRepository.ListAsync(new PlanetsSpecification(universe, planet));
            var result = results.FirstOrDefault();

            if (result == null) return NotFound();
            await _planetAsyncRepository.DeleteAsync(result);
            return NoContent();
        }
        
        [ExcludeFromCodeCoverage]
        [HttpPut("milky-way")]
        public async Task<ActionResult<IReadOnlyList<Planet>>> Create_MilkyWay()
        {
            // delete milky way first
            var planets = await _planetAsyncRepository.ListAsync(new PlanetsSpecification("Milky Way"));
            foreach (var planet in planets)
                await _planetAsyncRepository.DeleteAsync(planet);
            
            var milkyPlanets = new List<Planet>
            {
                new Planet { Name = "Mercury", Universe = "Milky Way" },
                new Planet { Name = "Venus", Universe = "Milky Way" },
                new Planet { Name = "Earth", Universe = "Milky Way" },
                new Planet { Name = "Mars", Universe = "Milky Way" },
                new Planet { Name = "Jupiter", Universe = "Milky Way" },
                new Planet { Name = "Saturn", Universe = "Milky Way" },
                new Planet { Name = "Uranus", Universe = "Milky Way" },
                new Planet { Name = "Neptune", Universe = "Milky Way" },
            };
            
            var results = new List<Planet>();
            foreach (var milky in milkyPlanets)
                results.Add(await _planetAsyncRepository.AddAsync(milky));

            return Ok(results);
        }
    }
}
