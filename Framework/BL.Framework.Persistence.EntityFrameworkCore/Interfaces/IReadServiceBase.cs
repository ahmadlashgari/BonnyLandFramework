using Ardalis.Specification;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.EntityFrameworkCore.Interfaces
{
    public interface IReadServiceBase<T> where T : class, IAggregateRoot
    {
        Task<List<T>> FindAllAsync(CancellationToken cancellationToken = default);
        Task<List<T>> FindAllBySpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FindBySpecAsync(ISingleResultSpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountBySpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyBySpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    }
}
