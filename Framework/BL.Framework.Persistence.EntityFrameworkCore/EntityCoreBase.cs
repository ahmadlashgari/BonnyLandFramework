using System;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
	public abstract class EntityCoreBase
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
