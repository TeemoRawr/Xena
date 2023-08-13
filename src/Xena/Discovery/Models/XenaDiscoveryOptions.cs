namespace Xena.Discovery.Models;

public class XenaDiscoveryOptions
{
    public TimeSpan RefreshServicesTimeThreshold { get; set; } = TimeSpan.FromMinutes(5);
}