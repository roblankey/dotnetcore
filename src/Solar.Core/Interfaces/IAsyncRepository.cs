using Solar.Core.Entities;

namespace Solar.Core.Interfaces
{
    // https://raw.githubusercontent.com/dotnet-architecture/eShopOnWeb/master/src/ApplicationCore/Interfaces/IAsyncRepository.cs
    public interface IAsyncRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : BaseEntity
    {
    }
}
