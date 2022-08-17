using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB.Interfaces
{
    public interface IReadRepository<T> where T : class, IAggregateRoot 
    {
        IMongoCollection<T> Collection { get; set; }
        Task<List<T>> FindAsync(int skip, int limit);
        Task<List<T>> FindAsync(int skip, int limit, Expression<Func<T, bool>> predicate);
        Task<List<T>> FindAsync(int skip, int limit, FilterDefinition<T> filter);
        Task<T> FindByIdAsync(string id);
        Task<T> FindByPredicateAsync(Expression<Func<T, bool>> predicate);
    }
}
