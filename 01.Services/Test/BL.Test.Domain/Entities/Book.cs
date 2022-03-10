using BL.Framework.Persistence.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BL.Test.Domain.Entities
{
    public class Book : EntityCoreBase, IAggregateRoot
    {
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
