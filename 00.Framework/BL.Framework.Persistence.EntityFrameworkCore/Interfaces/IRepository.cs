using Ardalis.Specification;

namespace BL.Framework.Persistence.EntityFrameworkCore.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
