using System.Threading.Tasks;
using Solar.Core.Entities;

namespace Solar.Core.Interfaces
{
    public interface IWriteRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
