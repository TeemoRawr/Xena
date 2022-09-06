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

var app = await builder.BuildAsync();

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

## Health Check
Xena framework allows agregation of all health checks in one which allows checking general health of the service in a simple manner.

### Usage
To add Helath check extensions you need to execute method `AddHealthChecks` in Xena application builder.

```
var app = builder
    .AddHealthChecks()
    .Build();
```

### Adding health check to application
If you want to add your health check service you need implement to `IXenaHealthCheck` and register it in DI. You can register many if you need more than one health check.
```
applicationBuilder.Services.AddSingleton<IXenaHealthCheck, MyHealthCheck>();
```

### Automatic health check discovery
Instead of registering all your health checks, you can enable auto discovery of all the services. To enable automatic health check discovery you need to enable it in configuration of health check.

```
var app = builder
    .AddHealthChecks(configurator =>
    {
        configurator.EnableAutoDiscoveryHealthChecks();
    })
    .Build();
```

## Discovery
Discovery service allows application to retrieve information about the other applications in ecosystem in an easy way.

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
Memory provider is default for Xena package and it allows setting the address of services manually

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

## Readiness
The Xena framework allows you to check whether the application is configured correctly by using readiness services.

### Usage
To add Readiness extensions you need execute method `AddReadiness` on Xena application builder.

```
var app = builder
    .AddReadiness(configurator =>
    {
        // configuration here
    })
    .Build();
```

### Adding readiness service to application
If you want to add your readiness service you need implement to `IXenaReadiness` and register it in DI. You can register many if you need more than one health check.
```
applicationBuilder.Services.AddSingleton<IXenaReadiness, MyReadiness>();
```


### Automatic readiness services discovery
Instead of registering all your readiness service, you can enable auto discovery of all the services. To enable automatic readiness services discovery you need to enable it in configuration of readiness.

```
var app = builder
    .AddReadiness(configurator =>
    {
        configurator.EnableAutoDiscoveryReadiness();
    })
    .Build();
```

# Contributing
(TODO)
