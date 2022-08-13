namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public abstract class EntityBase : EntityCoreBase
    {
        public bool Active { get; set; }
        public string CreatorUserId { get; set; }
        public string DeletorUserId { get; set; }
        //public ApplicationUser CreatorUser { get; set; }
    }
}
