using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.EntityFrameworkCore.Interfaces
{
    public interface IServiceBase<T> : IReadServiceBase<T> where T : class, IAggregateRoot
    {
        Task<T> AddAsync(T model, CancellationToken cancellationToken = default);
        Task<List<T>> AddRangeAsync(List<T> model, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<List<T>> UpdateRangeAsync(List<T> model, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task SoftDeleteRangeAsync(List<T> model, CancellationToken cancellationToken = default);
    }
}
