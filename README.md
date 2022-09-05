# Xena

Xena is a framework for .NET that supports the developer in creating an ecosystem that will be built from microservices applications.

# Installation
Xena is available on [NuGet](https://www.nuget.org/packages/Xena)
```
dotnet add package Xena
```

Use the `--version` option to specify a preview version to install.

# Basic usage
## Program.cs

You need to change `Program.cs` file to use framework properly

Example:
```
var builder = XenaFactory.Build(args);
var applicationBuilder = builder.WebApplicationBuilder;

applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
applicationBuilder.Services.AddRazorPages();

var app = builder.Build();

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
```

# Extensions

# Health Check
Xena framework allow agregate all health checks in one which allow check simply general health of service.

### Usage
To add Helath check extensions you need execute method `AddHealthChecks` on Xena application builder.
```
var app = builder
    .AddHealthChecks()
    .Build();
```

### Adding health check to service
If you want to add your health check service you need implement `IXenaHealthCheck` and register it in DI. You can register many if you need one more one health check.
```
applicationBuilder.Services.AddSingleton<IXenaHealthCheck, MyHealthCheck>();
```

### Automatic health check discovery
Instead of registration all your health checks, you can enable auto discovery of all the services. To enable automatic health check discovery you need enable it in configuration of health check.
```
var app = builder
    .AddHealthChecks(configurator =>
    {
        configurator.EnableAutoDiscoveryHealthChecks();
    })
    .Build();
```

## Discovery
Discovery service allows application in easy way to retrieve information of the rest of other applications in ecosystem.

### Usage
To add Discovery extensions you need execute method `AddDiscoveryServicesService` on Xena application builder.
```
var app = builder
    .AddDiscoveryServicesService(configurator =>
    {
        // configuration here
    })
    .Build();
```
### Providers
Default provider build in Xena package is memory provider, which allow set address of the services set manualy.

```
builder.AddDiscoveryServicesService(configurator =>
{
    configurator.AddMemoryProvider(new List<Service>
    {
        new Service
        {
            Id = "MyApplication",
            Name = "My application",
            Address = "localhost",
            Port = 4026
        }
    });
})
```

The other available providers are
* Xena.Discovery.Consul
* Xena.Discovery.EntityFramework

# Contributing
(TODO)
