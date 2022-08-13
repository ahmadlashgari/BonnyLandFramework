using Ardalis.Specification;

namespace BL.Framework.Persistence.EntityFrameworkCore.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
