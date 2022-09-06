namespace Xena.Readiness.Models;

public class XenaReadinessOptions
{
    public XenaReadinessBehavior BehaviorOnSuccessful { get; set; } = XenaReadinessBehavior.LogInformation;
    public XenaReadinessBehavior BehaviorOnWarning { get; set; } = XenaReadinessBehavior.LogError;

    public XenaReadinessBehavior BehaviorOnError { get; set; } =
        XenaReadinessBehavior.LogCritical | XenaReadinessBehavior.TerminateApplication;
}