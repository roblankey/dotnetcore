using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using ServiceStack.Aws.DynamoDb;
using Solar.Core.Entities;
using Solar.Core.Interfaces;
#pragma warning disable 1998

namespace Solar.Infrastructure.Repositories
{
    public class DynamoDbRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly IPocoDynamo _dynamoDb;

        protected DynamoDbRepository(IPocoDynamo pocoDynamo)
        {
            _dynamoDb = pocoDynamo;
        }

        [ExcludeFromCodeCoverage]
        public virtual async Task<T> AddAsync(T entity)
        {
            entity.Id = _dynamoDb.Sequences.Increment<T>();
            return _dynamoDb.PutItem(entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dynamoDb.DeleteItem<T>(entity.Id);
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return _dynamoDb.GetItem<T>(id);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return _dynamoDb.GetAll<T>();
        }

        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return _dynamoDb.FromScan(spec.Criterias.First()).Exec().ToList();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dynamoDb.PutItem(entity);
        }
    }
}
