using Microsoft.Extensions.DependencyInjection;
using Xena.HttpClient.Configuration;
using Xena.HttpClient.Factories;
using Xena.Startup;

namespace Xena.HttpClient
{
    public static class XenaHttpClientServicesExtensions
    {
        public static IXenaWebApplicationBuilder AddHttpClients(
            this IXenaWebApplicationBuilder webApplicationBuilder, 
            Action<IXenaHttpClientConfigurator> configurationAction)
        { 
            webApplicationBuilder.WebApplicationBuilder.Services.AddTransient<XenaHttpClientFactory>();

            var xenaHttpClientConfigurator = new XenaHttpClientConfigurator(webApplicationBuilder);
            configurationAction(xenaHttpClientConfigurator);

            return webApplicationBuilder;
        }
    }
}