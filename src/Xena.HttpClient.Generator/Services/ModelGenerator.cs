using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Services;

public class ModelGenerator
{
    private readonly PropertyGenerator _propertyGenerator;

    public ModelGenerator(PropertyGenerator propertyGenerator)
    {
        _propertyGenerator = propertyGenerator;
    }

    public List<ClientModel> Build(OpenApiDocument apiDocument, OpenApiDiagnostic apiDiagnostic)
    {
        var clientModels = new List<ClientModel>();

        foreach (var (schemaName, schema) in apiDocument.Components.Schemas)
        {
            var properties = schema.Properties
                .Select(p => new { Name = p.Key, Schema = p.Value })
                .Select(p => _propertyGenerator.Build(p.Name, p.Schema))
                .ToList();

            var clientModel = new ClientModel
            {
                Name = schemaName,
                Properties = properties
            };

            clientModels.Add(clientModel);
        }

        return clientModels;
    }

}