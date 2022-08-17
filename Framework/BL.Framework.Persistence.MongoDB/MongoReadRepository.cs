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

        public async Task<List<T>> FindAsync(int skip, int limit)
        {
            return await Collection.Find(_ => true).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<List<T>> FindAsync(int skip, int limit, Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<List<T>> FindAsync(int skip, int limit, FilterDefinition<T> filter)
        {
            return await Collection.Find<T>(filter).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<T> FindByIdAsync(string id)
        {
            FilterDefinition<T> filter = new BsonDocument("id", id);

            return await Collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<T> FindByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            return await Collection.Find(predicate).SingleOrDefaultAsync();
        }
    }
}
