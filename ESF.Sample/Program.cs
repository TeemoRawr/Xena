using Autofac.Extensions.DependencyInjection;
using ESF.Discovery;
using ESF.Discovery.Consul;
using ESF.Discovery.Consul.Configuration;
using ESF.HealthCheck;
using ESF.Startup;

var builder = EsfFactory.Build(args);

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
        configurator
            .AddHealthCheck()
            .AddConsulDiscover();
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