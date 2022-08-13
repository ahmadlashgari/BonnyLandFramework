using MongoDB.Driver;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.MongoDB.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IMongoCollection<T> Collection { get; set; }
        Task<T> InsertOneAsync(T entity);
    }
}
