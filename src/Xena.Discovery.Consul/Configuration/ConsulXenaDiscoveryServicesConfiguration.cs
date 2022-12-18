using System.ComponentModel.DataAnnotations;

namespace Xena.Discovery.Consul.Configuration;

public class ConsulXenaDiscoveryServicesConfiguration
{
    [Required]
    public string Host { get; set; } = null!;
    [Required]

    public string Id { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;
    
    public bool EnableHealthCheck { get; set; }
}