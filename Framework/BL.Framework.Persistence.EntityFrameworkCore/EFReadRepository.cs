using Ardalis.Specification.EntityFrameworkCore;
using BL.Framework.Persistence.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public class EFReadRepository<T, TDbContext> : RepositoryBase<T>, IReadRepository<T> where T : class, IAggregateRoot where TDbContext : DbContext
    {
        public EFReadRepository(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
