using System.Diagnostics.CodeAnalysis;
using Xena.Startup.Interfaces;

namespace Xena.Startup;

[ExcludeFromCodeCoverage]
public static class XenaFactory
{
    /// <summary>
    /// Prepare builder to use Xena framework
    /// </summary>
    /// <param name="args">
    /// Arguments provided by Main method
    /// </param>
    /// <returns>
    /// Returns <see cref="XenaWebApplicationBuilder"/>
    /// </returns>
    public static IXenaWebApplicationBuilder Build(string[] args)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(args);
        return new XenaWebApplicationBuilder(webApplicationBuilder);
    }
}
