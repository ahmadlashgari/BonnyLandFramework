using Ardalis.Specification.EntityFrameworkCore;
using BL.Framework.Persistence.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public class EFRepository<T, TDbContext> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot where TDbContext : DbContext
    {
        public EFRepository(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
