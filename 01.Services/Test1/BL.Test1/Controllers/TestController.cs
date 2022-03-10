using BL.Framework.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BL.Test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TestController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PushNotification()
        {
            await _publishEndpoint.Publish(new SmsNotificationSentEvent
            {
                Message = "Test",
                To = "09123901915"
            });

            return Ok();
        }
    }
}
