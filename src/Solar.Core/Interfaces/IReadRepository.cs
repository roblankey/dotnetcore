using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using Solar.Core.Entities;

namespace Solar.Core.Interfaces
{
    public interface IReadRepository<T> where T : BaseEntity
    {
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    }
}
