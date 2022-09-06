using Autofac.Extensions.DependencyInjection;
using Xena.Discovery.Consul.Configuration;
using Xena.HealthCheck;
using Xena.Readiness;
using Xena.Startup;

var builder = XenaFactory.Build(args);

var applicationBuilder = builder.WebApplicationBuilder;

applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
applicationBuilder.Services.AddRazorPages();

    applicationBuilder.Services.AddOptions<ConsulXenaDiscoveryServicesConfiguration>()
        .BindConfiguration("Consul");

var app = await builder
    .AddReadiness(p => p.EnableAutoDiscoveryReadiness())
    .AddHealthChecks(p => p.EnableAutoDiscoveryHealthChecks())
    .BuildAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();