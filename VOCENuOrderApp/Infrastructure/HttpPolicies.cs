using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http;

namespace VOCENuOrderApp.Infrastructure
{
    public static class HttpPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> DefaultRetry =>
            Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r => (int)r.StatusCode == 429 || (int)r.StatusCode >= 500)
                .WaitAndRetryAsync(6, attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt)) +
                    TimeSpan.FromMilliseconds(Random.Shared.Next(0, 250))
                );
    }
}
