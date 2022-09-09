using Autofac.Extensions.DependencyInjection;
using Xena.Discovery;
using Xena.Discovery.Consul;
using Xena.Discovery.Consul.Configuration;
using Xena.HealthCheck;
using Xena.Readiness;
using Xena.Startup;

var builder = XenaFactory.Build(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddRazorPages();

builder.Services.AddOptions<ConsulXenaDiscoveryServicesConfiguration>()
        .BindConfiguration("Consul");

var app = await builder
    .AddReadiness(p => p.EnableAutoDiscoveryReadiness())
    .AddHealthChecks(p => p.EnableAutoDiscoveryHealthChecks())
    .AddDiscovery(configurator =>
    {
        configurator.AddConsulProvider();
    })
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

await app.RunAsync();