using Xena.Readiness.Configurator;
using Xena.Startup.Interfaces;

namespace Xena.Readiness;

public static class XenaReadinessServiceExtensions
{
    /// <summary>
    /// Adds required services for Readiness feature
    /// </summary>
    /// <param name="webApplicationBuilder">
    /// The <see cref="IXenaWebApplicationBuilder"/> to add the services to.
    /// </param>
    /// <param name="configurationAction">
    /// Action to configure Readiness feature
    /// </param>
    /// <returns>
    /// The <see cref="IXenaWebApplicationBuilder"/> so that additional calls can be chained
    /// </returns>
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