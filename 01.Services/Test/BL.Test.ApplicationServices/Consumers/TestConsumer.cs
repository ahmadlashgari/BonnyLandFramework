using BL.Framework.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BL.Test.ApplicationServices.Consumers
{
    public class TestConsumer : IConsumer<SmsNotificationSentEvent>
    {
        private readonly ILogger<TestConsumer> _logger;

        public TestConsumer(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<TestConsumer>>();
        }

        public Task Consume(ConsumeContext<SmsNotificationSentEvent> context)
        {
            _logger.LogInformation("Message Consumed");

            return Task.CompletedTask;
        }
    }
}
