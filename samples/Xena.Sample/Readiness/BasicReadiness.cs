using Xena.Readiness.Interfaces;
using Xena.Readiness.Models;

namespace PatientOnline.Readiness;

public class BasicReadiness : IXenaReadiness
{
    public Task<XenaReadinessStatus> CheckAsync(IServiceProvider serviceScopeServiceProvider)
    {
        return Task.FromResult(XenaReadinessStatus.Successful);
    }
}