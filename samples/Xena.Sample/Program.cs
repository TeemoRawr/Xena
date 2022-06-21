using Autofac.Extensions.DependencyInjection;
using Xena.Discovery;
using Xena.Discovery.Consul;
using Xena.Discovery.Consul.Configuration;
using Xena.HealthCheck;
using Xena.Startup;

var builder = XenaFactory.Build(args);

builder.Configure(applicationBuilder =>
{
    applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    applicationBuilder.Services.AddRazorPages();

    applicationBuilder.Services.AddOptions<ConsulDiscoveryServicesConfiguration>()
        .BindConfiguration("Consul");
});

var app = builder
    .AddDiscoveryServicesService(configurator =>
    {
        ConsulDiscoveryServicesExtensions.AddConsulDiscover(configurator
                .AddHealthCheck());
    })
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