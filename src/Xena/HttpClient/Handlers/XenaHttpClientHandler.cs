using Xena.HttpClient.Handlers.Interfaces;

namespace Xena.HttpClient.Handlers;

internal class XenaHttpClientHandler : HttpClientHandler
{
    private readonly IXenaHttpClientHandler[] _xenaHttpClientHandlers;

    public XenaHttpClientHandler(IXenaHttpClientHandler[] xenaHttpClientHandlers)
    {
        _xenaHttpClientHandlers = xenaHttpClientHandlers;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return InvokeNext(0, request, cancellationToken);
    }

    private async Task<HttpResponseMessage> InvokeNext(int index, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_xenaHttpClientHandlers.Length > index)
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