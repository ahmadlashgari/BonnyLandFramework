using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BL.Framework.Persistence.MongoDB
{
    public abstract class EntityCoreBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("createdOn")]
        public DateTime CreatedOn { get; set; }

        [BsonElement("lastUpdatedOn")]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonElement("deletedOn")]
        public DateTime? DeletedOn { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
