using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using Solar.Core.Entities;

namespace Solar.Core.Interfaces
{
    public interface IReadRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    }
}
