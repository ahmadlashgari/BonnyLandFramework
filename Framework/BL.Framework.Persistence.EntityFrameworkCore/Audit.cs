using System;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public class Audit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedProperties { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}
