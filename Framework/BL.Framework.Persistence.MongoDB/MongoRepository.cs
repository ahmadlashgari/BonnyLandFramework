using BL.Framework.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        public IMongoCollection<T> Collection { get; set; }

        public async Task<T> InsertOneAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);

            return entity;
        }
    }
}
