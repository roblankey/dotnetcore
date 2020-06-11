using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using NSubstitute;
using ServiceStack.Aws.DynamoDb;
using Solar.Core.Entities;
using Solar.Core.Specifications;
using Solar.Infrastructure.Repositories;
using Xunit;

namespace Solar.UnitTests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class DynamoDbRepositoryTests
    {
        private readonly Faker _faker = new Faker();
        private readonly IPocoDynamo _pocoDynamo = Substitute.For<IPocoDynamo>();

        [Fact]
        public async Task Delete_Correctly_Responds_Without_Entity()
        {
            var planet = new Planet
            {
                Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix()
            };
            
            _pocoDynamo.DeleteItem<Planet>(planet.Id).Returns((Planet) null);
            
            var repo = new PlanetRepository(_pocoDynamo);
            await repo.DeleteAsync(planet);

            _pocoDynamo.Received().DeleteItem<Planet>(planet.Id);
        }
        
        [Fact]
        public async Task GetById_Correctly_Responds_With_Entity()
        {
            var planet = new Planet
            {
                Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix()
            };
            
            _pocoDynamo.GetItem<Planet>(planet.Id).Returns(planet);
            
            var repo = new PlanetRepository(_pocoDynamo);
            var response = await repo.GetByIdAsync(planet.Id);

            _pocoDynamo.Received().GetItem<Planet>(planet.Id);
            Assert.Same(planet, response);
        }
        
        [Fact]
        public async Task ListAll_Correctly_Responds_With_Entities()
        {
            var planets = new List<Planet>
            {
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix() },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix() },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix() },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix() }
            };
            
            _pocoDynamo.ScanAll<Planet>().Returns(planets);
            
            var repo = new PlanetRepository(_pocoDynamo);
            var response = await repo.ListAllAsync();

            _pocoDynamo.Received().ScanAll<Planet>();
            Assert.Equal(planets.Count, response.Count);
            Assert.Same(planets.First(), response.First());
        }
        
        [Fact]
        public async Task List_Correctly_Responds_With_Filtered_Entities()
        {
            var universe = _faker.Internet.DomainWord();
            var planets = new List<Planet>
            {
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = universe },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = universe },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = universe },
                new Planet { Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainWord() }
            };
            
            var spec = new PlanetsSpecification(universe);

            _pocoDynamo.GetTableMetadata(typeof(Planet)).Returns(new DynamoMetadataType
            {
                Name = "Planet"
            });
            var exp = Substitute.For<ScanExpression<Planet>>(_pocoDynamo);
            exp.Exec().Returns(planets.Where(spec.Criterias.First().Compile()));
            
            _pocoDynamo.FromScan(spec.Criterias.First()).Returns(exp);
            
            var repo = new PlanetRepository(_pocoDynamo);
            var response = await repo.ListAsync(spec);

            _pocoDynamo.Received().FromScan(spec.Criterias.First());
            Assert.Equal(3, response.Count);
        }
        
        [Fact]
        public async Task Update_Correctly_Responds_Without_Entity()
        {
            var planet = new Planet
            {
                Id = new Random().Next(), Name = _faker.Internet.DomainName(), Universe = _faker.Internet.DomainSuffix()
            };
            
            _pocoDynamo.PutItem(planet).Returns(planet);
            
            var repo = new PlanetRepository(_pocoDynamo);
            await repo.UpdateAsync(planet);

            _pocoDynamo.Received().PutItem(planet);
        }
    }
}
