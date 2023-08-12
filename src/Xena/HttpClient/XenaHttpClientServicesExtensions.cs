using Xena.HttpClient.Configuration;
using Xena.HttpClient.Factories;
using Xena.HttpClient.Interceptors;
using Xena.HttpClient.Interceptors.Interfaces;
using Xena.Startup.Interfaces;

namespace Xena.HttpClient
{
    public static class XenaHttpClientServicesExtensions
    {
        public static IXenaWebApplicationBuilder AddHttpClients(
            this IXenaWebApplicationBuilder webApplicationBuilder, 
            Action<IXenaHttpClientConfigurator> configurationAction)
        { 
            webApplicationBuilder.Services.AddTransient<XenaHttpClientFactory>();
            webApplicationBuilder.Services.AddTransient<IXenaHttpClientInterceptor, LoggerXenaHttpClientInterceptor>();
            webApplicationBuilder.Services.AddTransient<IXenaHttpClientInterceptor, TimeoutRetryXenaHttpClientInterceptor>();

            var xenaHttpClientConfigurator = new XenaHttpClientConfigurator(webApplicationBuilder);
            configurationAction(xenaHttpClientConfigurator);

            return webApplicationBuilder;
        }
    }
}