using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace BL.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public CacheController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetKey()
        {
            var value = await _distributedCache.GetStringAsync("Test");

            return Ok(value);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SetKey()
        {
            await _distributedCache.SetStringAsync("Test", "Test Value");

            return Ok();
        }
    }
}
