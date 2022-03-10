using BL.Framework.Persistence.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB
{
    public class MongoReadRepository<T> : IReadRepository<T> where T : class, IAggregateRoot
    {
        public IMongoCollection<T> Collection { get; set; }

        public async Task<List<T>> GetAllAsync()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            FilterDefinition<T> filter = new BsonDocument("id", id);

            return await Collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).SingleOrDefaultAsync();
        }
    }
}
