using Xena.Readiness.Models;

namespace Xena.Readiness.Interfaces;

public interface IXenaReadiness
{
    Task<XenaReadinessStatus> CheckAsync(IServiceProvider serviceScopeServiceProvider);
}