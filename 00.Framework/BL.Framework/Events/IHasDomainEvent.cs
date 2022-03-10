using System.Collections.Generic;

namespace BL.Framework.Events
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}
