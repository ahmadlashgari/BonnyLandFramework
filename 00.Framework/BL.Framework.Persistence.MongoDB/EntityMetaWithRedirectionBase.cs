using MongoDB.Bson.Serialization.Attributes;

namespace BL.Framework.Persistence.MongoDB
{
    public abstract class EntityMetaWithRedirectionBase : EntityMetaBase
    {
        [BsonElement("seoRedirectionType")]
        public SeoRedirectionType SeoRedirectionType { get; set; }

        [BsonElement("targetCategoryId")]
        public int? TargetCategoryId { get; set; }

        [BsonElement("targetProductId")]
        public int? TargetProductId { get; set; }
    }

    public enum SeoRedirectionType
    {
        PermanentRedirectionToCategory = 1,
        TemporaryRedirectionToCategory,
        PermanentRedirectionToAnotherProduct,
        TemporaryRedirectionToAnother,
        NoRedirection
    }
}
