using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Framework.Persistence.EntityFrameworkCore.Mappings
{
	public class EntityMetaBaseMap<T> : EntityBaseMap<T> where T : EntityMetaBase
    {
        public EntityMetaBaseMap(string tableName, string schemaName) : base(tableName, schemaName)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            // Seo Properties
            builder.Property(e => e.MetaTitle).HasMaxLength(150);
            builder.Property(e => e.MetaDescription).HasMaxLength(350);
            builder.Property(e => e.MetaKeywords).HasMaxLength(350);
            builder.Property(e => e.FriendlyUrl).HasMaxLength(150);

            base.Configure(builder);
        }
    }
}
