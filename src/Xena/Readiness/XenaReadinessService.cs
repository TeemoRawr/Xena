using Microsoft.Extensions.Options;
using Xena.Readiness.Interfaces;
using Xena.Readiness.Models;

namespace Xena.Readiness;

internal class XenaReadinessService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<XenaReadinessService> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IOptions<XenaReadinessOptions> _xenaReadinessConfiguration;

    public XenaReadinessService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<XenaReadinessService> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<XenaReadinessOptions> xenaReadinessConfiguration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _xenaReadinessConfiguration = xenaReadinessConfiguration;
    }

    public async Task CheckReadiness()
    {
        var xenaReadinessConfiguration = _xenaReadinessConfiguration.Value;

        using var serviceScope = _serviceScopeFactory.CreateScope();

        var xenaReadinesses = serviceScope.ServiceProvider.GetServices<IXenaReadiness>().ToList();

        foreach (var xenaReadiness in xenaReadinesses)
        {
            var status = await xenaReadiness.CheckAsync(serviceScope.ServiceProvider);

            switch (status)
            {
                case XenaReadinessStatus.Successful:
                    {
                        ExecuteBehavior(xenaReadiness, xenaReadinessConfiguration.BehaviorOnSuccessful, status);
                        break;
                    }
                case XenaReadinessStatus.Warning:
                    {
                        ExecuteBehavior(xenaReadiness, xenaReadinessConfiguration.BehaviorOnWarning, status);
                        break;
                    }
                case XenaReadinessStatus.Error:
                    {
                        ExecuteBehavior(xenaReadiness, xenaReadinessConfiguration.BehaviorOnError, status);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }
    }

    public static IEnumerable<XenaReadinessBehavior> GetFlags(XenaReadinessBehavior e)
    {
        return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).Select(en => (XenaReadinessBehavior)en);
    }

    private void ExecuteBehavior(IXenaReadiness xenaReadiness, XenaReadinessBehavior behavior, XenaReadinessStatus status)
    {
        var serviceName = xenaReadiness.GetType().FullName;
        var message = $"{serviceName} return {status} status";

        var flags = GetFlags(behavior);

        foreach (var flag in flags)
        {
            switch (flag)
            {
                case XenaReadinessBehavior.LogInformation:
                    _logger.LogInformation(message);
                    break;
                case XenaReadinessBehavior.LogWarning:
                    _logger.LogWarning(message);
                    break;
                case XenaReadinessBehavior.LogError:
                    _logger.LogError(message);
                    break;
                case XenaReadinessBehavior.LogCritical:
                    _logger.LogCritical(message);
                    break;
                case XenaReadinessBehavior.TerminateApplication:
                    _logger.LogCritical($"Application is going to terminate cause readiness {serviceName} throw status {status}");
                    _hostApplicationLifetime.StopApplication();
                    break;
                case XenaReadinessBehavior.Nothing:
                default:
                    continue;
            }
        }
    }
}