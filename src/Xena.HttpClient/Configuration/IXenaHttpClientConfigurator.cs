using Xena.HttpClient.Models;

namespace Xena.HttpClient.Configuration;

public interface IXenaHttpClientConfigurator
{
    IXenaHttpClientConfigurator AddHttpClient<THttpClient>() where THttpClient : IXenaHttpClient;
}