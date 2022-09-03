using Autofac.Extensions.DependencyInjection;
using Xena.Discovery;
using Xena.Discovery.Consul;
using Xena.Discovery.Consul.Configuration;
using Xena.Discovery.Memory;
using Xena.Discovery.Models;
using Xena.HealthCheck;
using Xena.Startup;

var builder = XenaFactory.Build(args);

var applicationBuilder = builder.WebApplicationBuilder;

applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
applicationBuilder.Services.AddRazorPages();

    applicationBuilder.Services.AddOptions<ConsulXenaDiscoveryServicesConfiguration>()
        .BindConfiguration("Consul");

var app = builder
    .AddHealthChecks()
    .Build();


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