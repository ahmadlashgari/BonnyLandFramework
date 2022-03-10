using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Framework.Persistence.EntityFrameworkCore.Mappings
{
	public class EntityCoreBaseMap<T> : IEntityTypeConfiguration<T> where T : EntityCoreBase
    {
        private string _tableName { get; }
        private string _schemaName { get; }

        public EntityCoreBaseMap(string tableName, string schemaName)
        {
            _tableName = tableName;
            _schemaName = schemaName;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            // Date Properties
            builder.Property(e => e.CreatedOn);
            builder.Property(e => e.LastUpdatedOn);
            builder.Property(e => e.DeletedOn);

            builder.ToTable(_tableName, _schemaName);
        }
    }
}
