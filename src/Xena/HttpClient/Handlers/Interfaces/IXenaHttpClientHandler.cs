namespace Xena.HttpClient.Handlers.Interfaces;

public interface IXenaHttpClientHandler
{
    // Task<HttpRequestMessage> InterceptRequest(
    //     HttpRequestMessage request,
    //     CancellationToken cancellationToken);
    //
    // Task<HttpResponseMessage> InterceptResponse(
    //     HttpResponseMessage response,
    //     CancellationToken cancellationToken);

    Task<HttpResponseMessage> Intercept(
        HttpRequestMessage request,
        Func<HttpRequestMessage, Task<HttpResponseMessage>> next);
}