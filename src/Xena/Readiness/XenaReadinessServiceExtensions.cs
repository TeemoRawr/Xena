using Xena.Readiness.Configurator;
using Xena.Startup.Interfaces;

namespace Xena.Readiness;

public static class XenaReadinessServiceExtensions
{
    public static IXenaWebApplicationBuilder AddReadiness(
        this IXenaWebApplicationBuilder webApplicationBuilder,
        Action<IXenaReadinessConfigurator>? configurationAction = null)
    {
        var xenaReadinessConfiguration = new XenaReadinessConfigurator(webApplicationBuilder);
        configurationAction?.Invoke(xenaReadinessConfiguration);

        webApplicationBuilder.Services.AddTransient<XenaReadinessService>();

        webApplicationBuilder.AddPostBuildAsyncAction(async p =>
        {
            var xenaReadinessService = p.Services.GetRequiredService<XenaReadinessService>();
            await xenaReadinessService.CheckReadiness();
        });

        return webApplicationBuilder;
    }
}