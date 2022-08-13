using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Framework.Persistence.EntityFrameworkCore.Mappings
{
	public class EntityMetaWithRedirectionBaseMap<T> : EntityMetaBaseMap<T> where T : EntityMetaWithRedirectionBase
    {
        public EntityMetaWithRedirectionBaseMap(string tableName, string schemaName) : base(tableName, schemaName)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }
    }
}