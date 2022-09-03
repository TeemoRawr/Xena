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
