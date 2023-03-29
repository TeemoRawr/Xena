using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.OpenApi.Models;
using TypeNameFormatter;
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

        internalModel = AddAttributes(internalModel, options);
        internalModel = AddSchemaDescription(internalModel);

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
                SyntaxFactory.ParseName(typeof(RequiredAttribute).GetFormattedName())
            );

            attributesToAdd.Add(requiredAttribute);
        }

        if (Schema.Deprecated)
        {
            var obsoleteAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(ObsoleteAttribute).GetFormattedName())
            );

            attributesToAdd.Add(obsoleteAttribute);
        }
        
        if (!attributesToAdd.Any())
        {
            return internalModel;
        }

        var newMember = internalModel.Member
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
            Member = newMember,
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
     
        var newMember = result.Member.WithLeadingTrivia(
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
            Member = newMember,
            ExtraObjectMembers = result.ExtraObjectMembers
        };
    }
}