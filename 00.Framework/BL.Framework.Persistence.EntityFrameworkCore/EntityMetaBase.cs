namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public abstract class EntityMetaBase : EntityBase
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string FriendlyUrl { get; set; }
    }
}
