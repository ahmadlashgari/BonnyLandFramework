using MassTransit;
using System.Collections.Generic;

namespace BL.Framework.AspNetCore.Options
{
    public class RabbitMqEndpointOption
    {
        public string QueueName { get; set; }
        public bool UseInMemoryOutbox { get; set; }
        public bool ConcurrentMessageDelivery { get; set; }
        public bool UseMessageRetry { get; set; }
        public int MessageRetryImmediate { get; set; }
        public List<string> Consumers { get; set; }
    }
}
