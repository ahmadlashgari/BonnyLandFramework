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
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate);
    }
}
