using Xena.Discovery.Interfaces;

namespace Xena.Discovery;

public class XenaDiscoveryStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            var xenaDiscoveryInitializeService = builder.ApplicationServices.GetRequiredService<IXenaDiscoveryInitializeService>();

            xenaDiscoveryInitializeService.InitializeAsync().GetAwaiter().GetResult();

            next(builder);
        };
    }
}