namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public abstract class EntityMetaWithRedirectionBase : EntityMetaBase
    {
        public SeoRedirectionType SeoRedirectionType { get; set; }
        public int? TargetCategoryId { get; set; }
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
