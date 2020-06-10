using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Ardalis.Specification;
using Solar.Core.Entities;
using Solar.Core.Interfaces;

namespace Solar.Infrastructure.Repositories
{
    public class DynamoRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName;

        protected DynamoRepository(IAmazonDynamoDB dynamoDb, string tableName)
        {
            _dynamoDb = dynamoDb;
            _tableName = tableName;
        }

        public Task<int> CountAsync(ISpecification<T> spec)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FirstAsync(ISpecification<T> spec)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            var response = await _dynamoDb.QueryAsync(new QueryRequest
            {
                TableName = _tableName
            });
            
            if (response == null) return new List<T>();
            return new List<T>();
        }

        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> AddAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
