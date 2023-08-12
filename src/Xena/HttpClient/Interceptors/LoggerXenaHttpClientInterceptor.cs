using Xena.HttpClient.Interceptors.Interfaces;

namespace Xena.HttpClient.Interceptors;

public class LoggerXenaHttpClientInterceptor : IXenaHttpClientInterceptor
{
    private readonly ILogger<LoggerXenaHttpClientInterceptor> _logger;

    public LoggerXenaHttpClientInterceptor(ILogger<LoggerXenaHttpClientInterceptor> logger)
    {
        _logger = logger;
    }

    public async Task<HttpResponseMessage> Intercept(HttpRequestMessage request, Func<HttpRequestMessage, Task<HttpResponseMessage>> next)
    {
        _logger.LogInformation($"Calling on address {request.RequestUri} (method {request.Method})");

        var response = await next(request);

        _logger.LogInformation($"Http response: {response.StatusCode}");

        return response;
    }
}
