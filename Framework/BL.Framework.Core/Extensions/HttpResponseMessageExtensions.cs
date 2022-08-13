using System.Net;
using System.Net.Http;

namespace BL.Framework.Core.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureNotFound(this HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new HttpRequestException($"Expected 404 Not Found but was {response.StatusCode}.");
            }
        }
    }
}
