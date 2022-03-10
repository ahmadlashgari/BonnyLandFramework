using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Framework.Persistence.EntityFrameworkCore.Mappings
{
	public class EntityBaseMap<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        private string _tableName { get; }
        private string _schemaName { get; }

        public EntityBaseMap(string tableName, string schemaName)
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

            // Relations
            //builder.HasOne(e => e.CreatorUser)
            //    .WithMany().HasForeignKey(e => e.CreatorUserId)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(_tableName, _schemaName);
        }
    }
}
