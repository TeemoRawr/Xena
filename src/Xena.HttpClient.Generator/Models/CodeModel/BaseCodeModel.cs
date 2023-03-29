using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Extensions;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public abstract class BaseCodeModel
{
    protected BaseCodeModel(string name, OpenApiSchema schema)
    {
        OriginalName = name;
        Schema = schema;
    }

    protected string OriginalName { get; }

    protected string NormalizedName => OriginalName.ToPascalCase();
    
    protected OpenApiSchema Schema { get; }
    
    protected abstract CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options);

    public CodeModelGenerationResult Generate()
    {
        return Generate(new CodeModelGenerateOptions());
    }
    
    public CodeModelGenerationResult Generate(CodeModelGenerateOptions options)
    {
        var internalModel = GenerateInternal(options);

        internalModel = AddSchemaDescription(internalModel);
        internalModel = AddAttributes(internalModel, options);
        
        return internalModel;
    }

    private CodeModelGenerationResult AddAttributes(
        CodeModelGenerationResult internalModel,
        CodeModelGenerateOptions generateOptions)
    {
        var attributesToAdd = new List<AttributeSyntax>();
        
        if (generateOptions.IsRequired)
        {
            var requiredAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(nameof(RequiredAttribute))
            );

            attributesToAdd.Add(requiredAttribute);
        }

        if (!attributesToAdd.Any())
        {
            return internalModel;
        }

        var newMember = internalModel.Memeber
            .WithAttributeLists(
                SyntaxFactory.List(
                    new List<AttributeListSyntax>
                    {
                        SyntaxFactory.AttributeList(
                            SyntaxFactory.SeparatedList(attributesToAdd)    
                        )
                    }
                )
            );

        return new CodeModelGenerationResult
        {
            Memeber = newMember,
            ExtraObjectMembers = internalModel.ExtraObjectMembers
        };
    }

    private CodeModelGenerationResult AddSchemaDescription(CodeModelGenerationResult result)
    {
        var workspace = (AdhocWorkspace)new AdhocWorkspace().AddProject("Project", "C#").Solution.Workspace;
        
        if (string.IsNullOrWhiteSpace(Schema.Description))
        {
            return result;
        }
     
        var newMember = result.Memeber.WithLeadingTrivia(
            SyntaxFactory.TriviaList(
                SyntaxFactory.Trivia(
                    SyntaxFactory.DocumentationComment(
                        SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText(Schema.Description)),
                        SyntaxFactory.XmlText(Environment.NewLine)
                    )
                ),
                SyntaxFactory.LineFeed
            )
        );
        
        newMember =  (MemberDeclarationSyntax)Formatter.Format(newMember, workspace);

        return new CodeModelGenerationResult
        {
            Memeber = newMember,
            ExtraObjectMembers = result.ExtraObjectMembers
        };
    }
}

public class CodeModelGenerationResult
{
    public MemberDeclarationSyntax Memeber { get; init; } = null!;

    public IReadOnlyList<MemberDeclarationSyntax> ExtraObjectMembers { get; init; } =
        new List<MemberDeclarationSyntax>(0);
}

public class CodeModelGenerateOptions
{
    public string Prefix { get; init; } = null!;
    public bool IsRequired { get; init; }
}