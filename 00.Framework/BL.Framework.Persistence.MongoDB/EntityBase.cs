using MongoDB.Bson.Serialization.Attributes;

namespace BL.Framework.Persistence.MongoDB
{
    public abstract class EntityBase : EntityCoreBase
    {
        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("creatorUserId")]
        public string CreatorUserId { get; set; }

        [BsonElement("deletorUserId")]
        public string DeletorUserId { get; set; }
        //public ApplicationUser CreatorUser { get; set; }
    }
}
