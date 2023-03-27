using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using System.Xml.Linq;

namespace Xena.HttpClient.Generator.Models;

public abstract class BaseCodeModel
{
    protected BaseCodeModel(string name, OpenApiSchema schema)
    {
        OriginalName = name;
        Schema = schema;
    }

    protected string OriginalName { get; }

    protected string NormalizedName =>
        (char.ToUpperInvariant(OriginalName[0]) + OriginalName.Substring(1)).Replace("_", string.Empty);
    protected OpenApiSchema Schema { get; }
    
    protected abstract MemberDeclarationSyntax GenerateInternal();

    public MemberDeclarationSyntax Generate()
    {
        var internalModel = GenerateInternal();

        internalModel = AddSchemaDescription(internalModel);

        return internalModel;
    }

    private MemberDeclarationSyntax AddSchemaDescription(MemberDeclarationSyntax internalModel)
    {
        if (string.IsNullOrWhiteSpace(Schema.Description))
        {
            return internalModel;
        }

        return internalModel.WithTriviaFrom(
            SyntaxFactory.DocumentationComment(
                SyntaxFactory.XmlSummaryElement(
                    SyntaxFactory.XmlText(Schema.Description)
                )
            )
        );
    }
}