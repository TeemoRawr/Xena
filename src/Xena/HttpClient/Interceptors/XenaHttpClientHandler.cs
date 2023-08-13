using System.Diagnostics.CodeAnalysis;
using Xena.HttpClient.Interceptors.Interfaces;

namespace Xena.HttpClient.Interceptors;

[ExcludeFromCodeCoverage]
internal class XenaHttpClientHandler : HttpClientHandler
{
    private readonly IXenaHttpClientInterceptor[] _xenaHttpClientHandlers;

    public XenaHttpClientHandler(IXenaHttpClientInterceptor[] xenaHttpClientHandlers)
    {
        _xenaHttpClientHandlers = xenaHttpClientHandlers;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return InvokeNext(0, request, cancellationToken);
    }

    private async Task<HttpResponseMessage> InvokeNext(int index, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_xenaHttpClientHandlers.Length - 1 < index)
        {
            return await base.SendAsync(request, cancellationToken);
        }
        
        var xenaHttpClientHandler = _xenaHttpClientHandlers[index];

        var result = await xenaHttpClientHandler.Intercept(request, async message =>
        {
            return await InvokeNext(index + 1, message, cancellationToken);
        });

        return result;
    }
}
