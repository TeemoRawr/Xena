using System.CommandLine;
using Autofac;
using Microsoft.OpenApi.Readers;
using Xena.HttpClient.Generator;
using Xena.HttpClient.Generator.Models;
using Xena.HttpClient.Generator.Services;
using Xena.HttpClient.Generator.Templates;

var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterModule<IocModule>();

var container = containerBuilder.Build();

var openApiFileNameArgument = new Argument<FileInfo?>("openApiFileName");

var rootCommand = new RootCommand();
rootCommand.AddArgument(openApiFileNameArgument);

var returnCode = 0;

rootCommand.SetHandler(async context =>
{
    var openApiFileName = context.ParseResult.GetValueForArgument(openApiFileNameArgument);

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

    var modelGenerator = container.Resolve<ModelGenerator>();
    var schemas = modelGenerator.Build(apiDocument, apiDiagnostic);

    var httpClientTemplate = new HttpClientTemplate
    {
        Model = new HttpClientTemplateModel
        {
            HttpClientName = "test",
            Schemas = schemas,
        }
    };

    var a = httpClientTemplate.TransformText();
});

await rootCommand.InvokeAsync(args);

Console.ReadLine();

return returnCode;