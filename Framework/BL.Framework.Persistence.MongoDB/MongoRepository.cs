using BL.Framework.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        public IMongoCollection<T> Collection { get; set; }

        public async Task<T> InsertOneAsync(T entity, InsertOneOptions insertOneOptions = null)
        {
            await Collection.InsertOneAsync(entity, insertOneOptions);

            return entity;
        }

        public async Task<List<T>> InsertManyAsync(IEnumerable<T> entities, InsertManyOptions insertManyOptions = null)
        {
            if (insertManyOptions == null)
            {
                insertManyOptions = new InsertManyOptions { IsOrdered = false };
            }

            await Collection.InsertManyAsync(entities, insertManyOptions);

            return entities.ToList();
        }

        public async Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null)
        {
            var result = await Collection.UpdateOneAsync<T>(filter, update, updateOptions);

            return result;
        }

        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null)
        {
            var result = await Collection.UpdateManyAsync<T>(filter, update, updateOptions);

            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null)
        {
            var result = await Collection.DeleteOneAsync<T>(filter, deleteOptions);

            return result;
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> filter, DeleteOptions deleteOptions = null)
        {
            var result = await Collection.DeleteManyAsync<T>(filter, deleteOptions);

            return result;
        }
    }
}
