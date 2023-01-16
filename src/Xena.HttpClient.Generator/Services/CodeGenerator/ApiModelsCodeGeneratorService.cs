using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xena.HttpClient.Generator.Models;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Services.CodeGenerator;

public class ApiModelsCodeGeneratorService
{
    private const string NamespaceName = "MyApplication"; 
    
    public string GenerateCode(List<ClientModel> clientModels)
    {
        var generatedClassModels = clientModels.Select(GenerateClientModelClass).ToArray();
        
        var codeNamespace = SF.NamespaceDeclaration(SF.ParseName(NamespaceName))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(generatedClassModels))
            .AddUsings(
                SF.UsingDirective(SF.ParseName("System.Collections.Generic")) 
            );

        var code = new StringWriter();
        
        codeNamespace.NormalizeWhitespace().WriteTo(code);

        return code.ToString();
    }

    private static ClassDeclarationSyntax GenerateClientModelClass(ClientModel clientModel)
    {
        var attributes = GetAttributes();

        var classPropertiesModels = clientModel.Properties
            .OrderBy(p => p.IsCollection)
            .Select(p =>
            {
                var model = SF.PropertyDeclaration(SF.ParseTypeName(p.FinalPropertyType), p.Name)
                    .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
                    .AddAccessorListAccessors(
                        SF.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)),
                        SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken))
                    );

                return model;
            })
            .ToList();
        
        var classClient = SF.ClassDeclaration(SF.Identifier(clientModel.Name))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(classPropertiesModels))
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .WithAttributeLists(new SyntaxList<AttributeListSyntax>(attributes));

        return classClient;
    }

    private static AttributeListSyntax GetAttributes()
    {
        var codeGeneratedAttributeDeclaration = SF.Attribute(
            SF.ParseName(typeof(GeneratedCodeAttribute).FullName),
            
            SF.AttributeArgumentList(SF.SeparatedList(new[]
            {
                SF.AttributeArgument(SF.LiteralExpression(SyntaxKind.StringLiteralExpression, SF.Literal("Xena.HttpClient.Generator"))),
                SF.AttributeArgument(SF.LiteralExpression(SyntaxKind.StringLiteralExpression, SF.Literal("1.0.0")))
            }))
        );

        return SF.AttributeList(SF.SingletonSeparatedList(codeGeneratedAttributeDeclaration));
    }
}