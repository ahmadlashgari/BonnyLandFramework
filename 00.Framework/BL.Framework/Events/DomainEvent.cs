using System;

namespace BL.Framework.Events
{
	public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            DateOccurred = DateTime.Now;
        }

        public DateTime DateOccurred { get; protected set; } = DateTime.Now;
    }
}
