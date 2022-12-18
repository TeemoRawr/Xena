using Autofac.Extensions.DependencyInjection;
using Xena.Discovery.Consul.Configuration;
using Xena.HealthCheck;
using Xena.MemoryBus;
using Xena.MemoryBus.Interfaces;
using Xena.Readiness;
using Xena.Sample.MemoryBus.Command;
using Xena.Sample.MemoryBus.Event;
using Xena.Sample.MemoryBus.Query;
using Xena.Startup;

var builder = XenaFactory.Build(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenLocalhost(5000);
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllers();

builder.Services.AddOptions<ConsulXenaDiscoveryServicesConfiguration>()
        .BindConfiguration("Consul");

builder.Services.AddTransient<IXenaCommandHandler<SimpleCommand>, SimpleCommandHandler>();
builder.Services.AddTransient<IXenaQueryHandler<SimpleQuery, int>, SimpleQueryHandler>();
builder.Services.AddTransient<IXenaEventHandler<SimpleEvent>, FirstSimpleEventHandler>();
builder.Services.AddTransient<IXenaEventHandler<SimpleEvent>, SecondSimpleEventHandler>();

var app = await builder
    .AddReadiness(p => p.EnableAutoDiscoveryReadiness())
    .AddHealthChecks(p => p.EnableAutoDiscoveryHealthChecks())
    .AddMemoryBus()    
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

app.UseEndpoints(routeBuilder =>
{
    routeBuilder.MapControllers();
});

await app.RunAsync();