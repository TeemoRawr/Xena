using Polly;
using System.Net;
using Xena.HttpClient.Interceptors.Interfaces;

namespace Xena.HttpClient.Interceptors;

public class TimeoutRetryXenaHttpClientInterceptor : IXenaHttpClientInterceptor
{
    private readonly ILogger<TimeoutRetryXenaHttpClientInterceptor> _logger;

    public TimeoutRetryXenaHttpClientInterceptor(ILogger<TimeoutRetryXenaHttpClientInterceptor> logger)
    {
        _logger = logger;
    }

    public async Task<HttpResponseMessage> Intercept(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next)
    {
        var result = await Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(
                5,
                p => TimeSpan.FromSeconds(5),
                (e, p) => _logger.LogInformation("Trying once again call"))
            .ExecuteAsync(async () => await next(request));

        return result;
    }
}