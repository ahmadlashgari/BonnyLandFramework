using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB.Interfaces
{
    public interface IServiceBase<T> : IReadServiceBase<T> where T : class, IAggregateRoot
    {
        Task<T> InsertOneAsync(T entity, InsertOneOptions insertOneOptions = null);
        Task<List<T>> InsertManyAsync(IEnumerable<T> entities, InsertManyOptions insertManyOptions = null);
        Task<ReplaceOneResult> ReplaceOneAsync(Expression<Func<T, bool>> filter, T entity, ReplaceOptions replaceOptions = null);
        Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<T> filter, T entity, ReplaceOptions replaceOptions = null);
        Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null);
        Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null);
        Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null);
        Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null);
    }
}
