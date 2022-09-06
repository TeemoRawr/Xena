namespace Xena.Readiness.Models;

[Flags]
public enum XenaReadinessBehavior : short
{
    Nothing = 0,
    LogInformation = 1,
    LogWarning = 2,
    LogError = 4,
    LogCritical = 8,
    TerminateApplication = 16
}