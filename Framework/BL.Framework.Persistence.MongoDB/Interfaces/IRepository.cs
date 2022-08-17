using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IMongoCollection<T> Collection { get; set; }
        Task<T> InsertOneAsync(T entity, InsertOneOptions insertOneOptions = null);
        Task<List<T>> InsertManyAsync(IEnumerable<T> entities, InsertManyOptions insertManyOptions = null);
        Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null);
        Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null);
        Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null);
        Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null);
    }
}
