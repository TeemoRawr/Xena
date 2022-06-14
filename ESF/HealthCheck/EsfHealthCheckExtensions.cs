using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using ESF.Startup;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace ESF.HealthCheck;

[ExcludeFromCodeCoverage]
public static class EsfHealthCheckExtensions
{
    public static IEsfWebApplicationBuilder AddHealthChecks(this IEsfWebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.WebApplicationBuilder.Services.AddHealthChecks()
            .AddCheck<EsfHealthCheck>("Esf Health Check", tags: new[] { "esf-health-check" });

        webApplicationBuilder.AddPostBuildAction(application =>
        {
            application.UseHealthChecks("/esf-health-check", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("esf-health-check"),
                ResponseWriter = async (context, healthReport) =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";

                    var options = new JsonWriterOptions { Indented = true };

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

                                JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object));
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