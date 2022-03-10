using BL.Framework.Persistence.MongoDB;

namespace BL.Test.Infrastructure.Persistence
{
    public class MongoReadRepositoryBase<T> : MongoReadRepository<T> where T : class, IAggregateRoot
    {
    }
}
