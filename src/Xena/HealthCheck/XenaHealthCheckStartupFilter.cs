using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Xena.HealthCheck;

public class XenaHealthCheckStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            builder.UseHealthChecks("/xena-health-check", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("xena-health-check")
            });
            
            next(builder);
        };
    }
}