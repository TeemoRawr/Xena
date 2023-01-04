using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Xena.HealthCheck.Configuration;
using Xena.Startup.Interfaces;

namespace Xena.HealthCheck;

public static class XenaHealthCheckExtensions
{
    /// <summary>
    /// Adds required services for Health Check feature
    /// </summary>
    /// <param name="webApplicationBuilder">
    /// The <see cref="IXenaWebApplicationBuilder"/> to add the services to.
    /// </param>
    /// <param name="configurationAction">
    /// Action to configure Health Check feature
    /// </param>
    /// <returns>
    /// The <see cref="IXenaWebApplicationBuilder"/> so that additional calls can be chained
    /// </returns>
    public static IXenaWebApplicationBuilder AddHealthChecks(
        this IXenaWebApplicationBuilder webApplicationBuilder,
        Action<IXenaHealthCheckConfigurator>? configurationAction = null)
    {
        var healthCheckConfigurator = new XenaHealthCheckConfigurator(webApplicationBuilder);
        configurationAction?.Invoke(healthCheckConfigurator);

        webApplicationBuilder.Services.AddHealthChecks()
            .AddCheck<XenaHealthCheckService>("Xena Health CheckAsync", tags: new[] { "xena-health-check" });

        webApplicationBuilder.AddPostBuildAction(application =>
        {
            application.UseHealthChecks("/xena-health-check", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("xena-health-check"),
                ResponseWriter = async (context, healthReport) =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";

                    var options = new JsonWriterOptions
                    {
                        Indented = true
                    };

                    var jsonSerializerOptions = new JsonSerializerOptions
                    {
                        Converters =
                        {
                            new JsonStringEnumConverter()
                        }
                    };

                    using var memoryStream = new MemoryStream();
                    await using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
                    {
                        jsonWriter.WriteStartObject();
                        jsonWriter.WriteString("status", healthReport.Status.ToString());
                        jsonWriter.WriteStartObject("results");

                        foreach (var healthReportEntry in healthReport.Entries)
                        {
                            jsonWriter.WriteStartObject(healthReportEntry.Key);
                            jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
                            jsonWriter.WriteString("description", healthReportEntry.Value.Description);
                            jsonWriter.WriteStartObject("internalHealthChecks");

                            foreach (var item in healthReportEntry.Value.Data)
                            {
                                jsonWriter.WritePropertyName(item.Key);

                                JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object), jsonSerializerOptions);
                            }

                            jsonWriter.WriteEndObject();
                            jsonWriter.WriteEndObject();
                        }

                        jsonWriter.WriteEndObject();
                        jsonWriter.WriteEndObject();
                    }

                    await context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            });
        });

        return webApplicationBuilder;
    }
}