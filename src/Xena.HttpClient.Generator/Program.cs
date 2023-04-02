using System.CommandLine;
using Autofac;
using Microsoft.OpenApi.Readers;
using Xena.HttpClient.Generator;
using Xena.HttpClient.Generator.Parsers;

var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterModule<IocModule>();

await using var container = containerBuilder.Build();

var openApiFileNameArgument = new Argument<FileInfo?>("openApiFileName");
var ignoreErrorsOption = new Option<bool>("--ignore-errors", () => false);

var rootCommand = new RootCommand();
rootCommand.AddArgument(openApiFileNameArgument);
rootCommand.AddOption(ignoreErrorsOption);

var returnCode = 0;

rootCommand.SetHandler(async context =>
{
    var openApiFileName = context.ParseResult.GetValueForArgument(openApiFileNameArgument);
    var ignoreErrors = context.ParseResult.GetValueForOption(ignoreErrorsOption);

    if (openApiFileName is null)
    {
        throw new ArgumentNullException(nameof(openApiFileName));
    }
    
    if (!openApiFileName.Exists)
    {
        Console.WriteLine($"File {openApiFileName.FullName} does not exists");
        returnCode = 1;
        return;
    }

    var fileContent = await File.ReadAllTextAsync(openApiFileName.FullName);

    var reader = new OpenApiStringReader();
    var apiDocument = reader.Read(fileContent, out var apiDiagnostic);

    if (apiDiagnostic.Errors.Any())
    {
        Console.Error.WriteLine($"OpenApi document is invalid. Found {apiDiagnostic.Errors.Count} errors");
        foreach (var error in apiDiagnostic.Errors)
        {
            Console.Error.WriteLine($"{error.Pointer}: {error.Message}");
        }

        if (!ignoreErrors)
        {
            returnCode = 1;
            return;
        }
    }

    var openApiParser = new OpenApiParser();
    var source = openApiParser.Generate(apiDocument);

    await File.WriteAllTextAsync($"{openApiFileName.FullName}.cs", source);
});

await rootCommand.InvokeAsync(args);

return returnCode;