using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using ServiceStack.Aws.DynamoDb;
using Solar.Core.Entities;
using Solar.Infrastructure.Repositories;

namespace Solar.IntegrationTests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FakePlanetRepository : DynamoDbRepository<Planet>
    {
        private readonly Dictionary<long, Planet> _planets = new Dictionary<long, Planet>
        {
            { 1, new Planet { Id = 1, Name = "Earth", Universe = "Milky Way" } },
            { 2, new Planet { Id = 2, Name = "Mars", Universe = "Milky Way" } },
            { 3, new Planet { Id = 3, Name = "Tatooine", Universe = "A Galaxy Far, Far Away" } }
        };
        
        public FakePlanetRepository(IPocoDynamo pocoDynamo) : base(pocoDynamo)
        {
        }

        public override Task<Planet> AddAsync(Planet entity)
        {
            _planets.Add(entity.Id, entity);
            return Task.FromResult(entity);
        }

        public override async Task<IReadOnlyList<Planet>> ListAsync(ISpecification<Planet> spec)
        {
            var all = await ListAllAsync();
            var exp = spec.Criterias.First().Compile();
            return all.Where(exp).ToList();
        }

        public override Task DeleteAsync(Planet entity)
        {
            _planets.Remove(entity.Id);
            return Task.CompletedTask;
        }

        public override async Task UpdateAsync(Planet entity)
        {
            if (_planets.ContainsKey(entity.Id))
            {
                _planets[entity.Id] = entity;
            }
            else
            {
                await AddAsync(entity);
            }
        }

        public override Task<IReadOnlyList<Planet>> ListAllAsync()
        {
            var all = (IReadOnlyList<Planet>) _planets.Select(entry => entry.Value).ToList();
            return Task.FromResult(all);
        }

        public override Task<Planet> GetByIdAsync(long id)
        {
            return Task.FromResult(_planets.ContainsKey(id) ? _planets[id] : null);
        }
    }
}
