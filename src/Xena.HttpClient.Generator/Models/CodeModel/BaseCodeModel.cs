﻿using System.ComponentModel.DataAnnotations;
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
    
    protected abstract CodeModelGenerationResult<MemberDeclarationSyntax> GenerateInternal(CodeModelGenerateOptions options);

    public CodeModelGenerationResult<MemberDeclarationSyntax> Generate()
    {
        return Generate(new CodeModelGenerateOptions());
    }
    
    public CodeModelGenerationResult<MemberDeclarationSyntax> Generate(CodeModelGenerateOptions options)
    {
        var internalModel = GenerateInternal(options);

        if (internalModel.Member is not null)
        {
            internalModel = AddAttributes(internalModel, options);
            internalModel = AddSchemaDescription(internalModel);
        }

        return internalModel;
    }

    private CodeModelGenerationResult<MemberDeclarationSyntax> AddAttributes(
        CodeModelGenerationResult<MemberDeclarationSyntax> internalModel,
        CodeModelGenerateOptions generateOptions)
    {
        var existingAttributes = internalModel
            .Member?
            .AttributeLists
            .SelectMany(p => p.Attributes) ?? new List<AttributeSyntax>();

        var attributesToAdd = new List<AttributeSyntax>();
        attributesToAdd.AddRange(existingAttributes);
        
        if (generateOptions.IsRequired)
        {
            var requiredAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(RequiredAttribute).GetNiceName())
            );

            attributesToAdd.Add(requiredAttribute);
        }

        if (Schema.Deprecated)
        {
            var obsoleteAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(ObsoleteAttribute).GetNiceName())
            );

            attributesToAdd.Add(obsoleteAttribute);
        }
        
        if (!attributesToAdd.Any())
        {
            return internalModel;
        }

        var newMember = internalModel.Member!
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

        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = newMember,
            ExtraObjectMembers = internalModel.ExtraObjectMembers
        };
    }

    private CodeModelGenerationResult<MemberDeclarationSyntax> AddSchemaDescription(
        CodeModelGenerationResult<MemberDeclarationSyntax> result)
    {
        var workspace = (AdhocWorkspace)new AdhocWorkspace().AddProject("Project", "C#").Solution.Workspace;
        
        if (string.IsNullOrWhiteSpace(Schema.Description))
        {
            return result;
        }
     
        var newMember = result.Member!.WithLeadingTrivia(
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

        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = newMember,
            ExtraObjectMembers = result.ExtraObjectMembers
        };
    }
}