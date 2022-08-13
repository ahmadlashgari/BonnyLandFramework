using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Framework.Persistence.EntityFrameworkCore.Mappings
{
	public class ValueObjectBaseMap<T> : IEntityTypeConfiguration<T> where T : ValueObjectBase
    {
        private string _tableName { get; }
        private string _schemaName { get; }

        public ValueObjectBaseMap(string tableName, string schemaName)
        {
            _tableName = tableName;
            _schemaName = schemaName;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.ToTable(_tableName, _schemaName);
        }
    }
}
