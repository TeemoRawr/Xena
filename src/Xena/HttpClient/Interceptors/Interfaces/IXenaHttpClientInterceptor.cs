namespace Xena.HttpClient.Interceptors.Interfaces;

public interface IXenaHttpClientInterceptor
{
    Task<HttpResponseMessage> Intercept(
        HttpRequestMessage request,
        Func<HttpRequestMessage, Task<HttpResponseMessage>> next);
}