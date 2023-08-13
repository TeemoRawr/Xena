using Xena.HealthCheck.Configuration;
using Xena.Startup.Interfaces;

namespace Xena.HealthCheck;

public static class XenaHealthCheckExtensions
{
    public static IXenaWebApplicationBuilder AddHealthChecks(
        this IXenaWebApplicationBuilder webApplicationBuilder,
        Action<IXenaHealthCheckConfigurator>? configurationAction = null)
    {
        var healthCheckConfigurator = new XenaHealthCheckConfigurator(webApplicationBuilder);
        configurationAction?.Invoke(healthCheckConfigurator);

        webApplicationBuilder.Services.AddHealthChecks()
            .AddCheck<XenaHealthCheckService>("Xena Health CheckAsync", tags: new[] { "xena-health-check" });

        webApplicationBuilder.Services.AddTransient<IStartupFilter, XenaHealthCheckStartupFilter>();

        return webApplicationBuilder;
    }
}