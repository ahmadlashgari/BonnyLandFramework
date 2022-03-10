using MongoDB.Bson.Serialization.Attributes;

namespace BL.Framework.Persistence.MongoDB
{
    public abstract class EntityMetaBase : EntityBase
    {
        [BsonElement("metaTitle")]
        public string MetaTitle { get; set; }

        [BsonElement("metaDescription")]
        public string MetaDescription { get; set; }

        [BsonElement("metaKeywords")]
        public string MetaKeywords { get; set; }

        [BsonElement("friendlyUrl")]
        public string FriendlyUrl { get; set; }
    }
}
