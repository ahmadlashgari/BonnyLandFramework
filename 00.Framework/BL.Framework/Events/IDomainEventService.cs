using System.Threading.Tasks;

namespace BL.Framework.Events
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
