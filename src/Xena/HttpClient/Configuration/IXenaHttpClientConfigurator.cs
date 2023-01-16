using Xena.HttpClient.Models;

namespace Xena.HttpClient.Configuration;

public interface IXenaHttpClientConfigurator
{
    IXenaHttpClientConfigurator AddHttpClient<THttpClient>(
        Func<HttpRequestMessage, Task<string>>? authorizationHeaderFunc = null) 
        where THttpClient : IXenaHttpClient;
}