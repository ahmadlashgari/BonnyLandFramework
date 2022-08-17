using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Framework.Core.Extensions
{
    public static class RestClientExtensions
    {
        public static async Task<RestResponse<T>> ExecuteWithRetryAsync<T>(this RestClient client, RestRequest request) where T : class
        {
            return await GetRetryPolicy<T>().ExecuteAsync(() => client.ExecuteAsync<T>(request));
        }

        public static async Task<RestResponse> ExecuteWithRetryAsync(this RestClient client, RestRequest request)
        {
            return await GetRetryPolicy().ExecuteAsync(() => client.ExecuteAsync(request));
        }

        private static AsyncRetryPolicy<RestResponse<T>> GetRetryPolicy<T>() where T : class
        {
            return Policy.HandleResult<RestResponse<T>>(r => new List<int> { 500, 502, 503, 504 }.Contains((int)r.StatusCode)).OrResult(r => new List<int> { 500, 502, 503 }.Contains((int)r.StatusCode)).WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }

        private static AsyncRetryPolicy<RestResponse> GetRetryPolicy()
        {
            return Policy.HandleResult<RestResponse>(r => new List<int> { 500, 502, 503, 504 }.Contains((int)r.StatusCode)).OrResult(r => new List<int> { 500, 502, 503 }.Contains((int)r.StatusCode)).WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }
    }
}
