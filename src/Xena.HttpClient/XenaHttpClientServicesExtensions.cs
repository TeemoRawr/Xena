using Xena.HttpClient.Configuration;
using Xena.Startup;

namespace Xena.HttpClient
{
    public static class XenaHttpClientServicesExtensions
    {
        public static IXenaWebApplicationBuilder AddHttpClients(
            this IXenaWebApplicationBuilder webApplicationBuilder, 
            Action<IXenaHttpClientConfigurator> configurationAction)
        {
            var xenaHttpClientConfigurator = new XenaHttpClientConfigurator(webApplicationBuilder);
            configurationAction(xenaHttpClientConfigurator);

            return webApplicationBuilder;
        }
    }
}