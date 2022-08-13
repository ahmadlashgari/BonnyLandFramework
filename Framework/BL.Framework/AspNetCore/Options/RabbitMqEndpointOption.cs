using System.Collections.Generic;

namespace BL.Framework.AspNetCore.Options
{
    public class RabbitMqEndpointOption
    {
        public string QueueName { get; set; }
        public bool UseMessageRetry { get; set; }
        public int MessageRetryCount { get; set; }
        public int MessageRetryIntervalSeconds { get; set; }
        public int PrefetchCount { get; set; }
        public bool Durable { get; set; }
        public string ExchangeType { get; set; }
        public bool UseConcurrencyLimit { get; set; }
        public int ConcurrentMessageLimit { get; set; }
        public bool UseCircuitBreaker { get; set; }
        public List<string> Consumers { get; set; }
    }
}
