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

# Contributing
(TODO)
