using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Omega.SourceGenerators;

[Generator]
public class InjectableSourceGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor MultipleInterfacesDescriptor = new(
        "OMEGA001",
        "Multiple interface implementation detected",
        "Class '{0}' implements multiple interfaces. Please use interface injection instead.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor NoImplementationDescriptor = new(
        "OMEGA002",
        "Interface without implementation",
        "Interface '{0}' has no implementing classes.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register the attribute source with enums
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "InjectableAttribute.g.cs",
            SourceText.From(@"
using System;

namespace Omega.SourceGenerators.Attributes;

public enum InjectionScope
{
    Transient,
    Scoped,
    Singleton
}

[Flags]
public enum Platform
{
    None = 0,
    iOS = 1 << 0,
    Android = 1 << 1,
    Windows = 1 << 2,
    MacCatalyst = 1 << 3,
    All = iOS | Android | Windows | MacCatalyst
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class InjectableAttribute : Attribute
{
    public InjectionScope Scope { get; }
    public Platform Platform { get; }

    public InjectableAttribute(InjectionScope scope, Platform platform)
    {
        Scope = scope;
        Platform = platform;
    }
}", Encoding.UTF8)));

        // Find all classes and interfaces with the Injectable attribute
        var injectableTypes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax or InterfaceDeclarationSyntax,
                transform: (context, _) => GetInjectableType(context))
            .Where(x => x != null)
            .Collect();

        // Generate the service registration code
        context.RegisterSourceOutput(injectableTypes, GenerateServiceRegistration);
    }

    private static InjectableTypeInfo? GetInjectableType(GeneratorSyntaxContext context)
    {
        var node = context.Node;
        var semanticModel = context.SemanticModel;

        // Get the type declaration syntax
        TypeDeclarationSyntax? typeDeclaration = node switch
        {
            ClassDeclarationSyntax classDeclaration => classDeclaration,
            InterfaceDeclarationSyntax interfaceDeclaration => interfaceDeclaration,
            _ => null
        };

        if (typeDeclaration == null)
            return null;

        // Check if the type has the Injectable attribute
        var injectableAttribute = typeDeclaration.AttributeLists
            .SelectMany(x => x.Attributes)
            .FirstOrDefault(x => semanticModel.GetSymbolInfo(x).Symbol?.ContainingType.Name == "InjectableAttribute" &&
                               semanticModel.GetSymbolInfo(x).Symbol?.ContainingNamespace.ToDisplayString() == "Omega.SourceGenerators.Attributes");

        if (injectableAttribute == null)
            return null;

        // Get the type symbol
        INamedTypeSymbol? typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);

        if (typeSymbol == null)
            return null;

        // Get the attribute arguments
        var attributeData = typeSymbol.GetAttributes()
            .First(x => x.AttributeClass?.Name == "InjectableAttribute" &&
                       x.AttributeClass?.ContainingNamespace.ToDisplayString() == "Omega.SourceGenerators.Attributes");

        var scope = (InjectionScope)attributeData.ConstructorArguments[0].Value!;
        var platform = (Platform)attributeData.ConstructorArguments[1].Value!;

        // For classes, check if they implement multiple interfaces
        if (typeSymbol.TypeKind == TypeKind.Class)
        {
            var interfaces = typeSymbol.Interfaces.ToImmutableArray();
            if (interfaces.Length > 1)
            {
                // Store the location for later diagnostic reporting
                return new InjectableTypeInfo(
                    typeSymbol.ToDisplayString(),
                    typeSymbol.TypeKind == TypeKind.Interface,
                    typeSymbol.Interfaces.Select(x => x.ToDisplayString()).ToImmutableArray(),
                    scope,
                    platform,
                    node.GetLocation(),
                    true);
            }
        }

        return new InjectableTypeInfo(
            typeSymbol.ToDisplayString(),
            typeSymbol.TypeKind == TypeKind.Interface,
            typeSymbol.Interfaces.Select(x => x.ToDisplayString()).ToImmutableArray(),
            scope,
            platform,
            null,
            false);
    }

    private static void GenerateServiceRegistration(SourceProductionContext context, ImmutableArray<InjectableTypeInfo> injectableTypes)
    {
        if (injectableTypes.IsEmpty)
            return;

        // Report diagnostics for types with multiple interfaces
        foreach (var type in injectableTypes.Where(x => x.HasMultipleInterfaces && x.Location != null))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                MultipleInterfacesDescriptor,
                type.Location,
                type.ClassName));
        }

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("// <auto-generated/>");
        sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        sourceBuilder.AppendLine("using Omega.SourceGenerators.Attributes;");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine("namespace Omega;");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine("public static partial class ServiceCollectionExtensions");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine("    public static IServiceCollection AddInjectableServices(this IServiceCollection services)");
        sourceBuilder.AppendLine("    {");

        // Group types by platform to avoid duplicate platform conditions
        var platformGroups = injectableTypes
            .GroupBy(x => x.Platform)
            .OrderBy(x => x.Key);

        foreach (var platformGroup in platformGroups)
        {
            sourceBuilder.AppendLine($"#if {GetPlatformCondition(platformGroup.Key)}");

            foreach (var injectableType in platformGroup)
            {
                if (injectableType.IsInterface)
                {
                    // For interfaces, we need to find implementing classes
                    var implementingClasses = injectableTypes
                        .Where(x => !x.IsInterface && x.Interfaces.Contains(injectableType.ClassName))
                        .ToImmutableArray();

                    if (implementingClasses.IsEmpty)
                    {
                        // Report a diagnostic for interface without implementation
                        context.ReportDiagnostic(Diagnostic.Create(
                            NoImplementationDescriptor,
                            injectableType.Location,
                            injectableType.ClassName));
                        continue;
                    }

                    foreach (var implementingClass in implementingClasses)
                    {
                        var registration = GetRegistrationMethod(implementingClass.Scope);
                        sourceBuilder.AppendLine($"        services.{registration}<{injectableType.ClassName}, {implementingClass.ClassName}>();");
                    }
                }
                else
                {
                    // For classes, register with their interfaces or as concrete type
                    if (injectableType.Interfaces.Length == 1)
                    {
                        // Single interface implementation
                        var registration = GetRegistrationMethod(injectableType.Scope);
                        sourceBuilder.AppendLine($"        services.{registration}<{injectableType.Interfaces[0]}, {injectableType.ClassName}>();");
                    }
                    else if (injectableType.Interfaces.IsEmpty)
                    {
                        // No interfaces, register as concrete type
                        var registration = GetRegistrationMethod(injectableType.Scope);
                        sourceBuilder.AppendLine($"        services.{registration}<{injectableType.ClassName}>();");
                    }
                    // Multiple interfaces are handled by interface registration above
                }
            }

            sourceBuilder.AppendLine("#endif");
            sourceBuilder.AppendLine();
        }

        sourceBuilder.AppendLine("        return services;");
        sourceBuilder.AppendLine("    }");
        sourceBuilder.AppendLine("}");

        context.AddSource("InjectableServices.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    private static string GetPlatformCondition(Platform platform)
    {
        var conditions = new List<string>();

        if ((platform & Platform.iOS) != 0)
            conditions.Add("IOS");
        if ((platform & Platform.Android) != 0)
            conditions.Add("ANDROID");
        if ((platform & Platform.Windows) != 0)
            conditions.Add("WINDOWS");
        if ((platform & Platform.MacCatalyst) != 0)
            conditions.Add("MACCATALYST");

        return string.Join(" || ", conditions);
    }

    private static string GetRegistrationMethod(InjectionScope scope) => scope switch
    {
        InjectionScope.Transient => "AddTransient",
        InjectionScope.Scoped => "AddScoped",
        InjectionScope.Singleton => "AddSingleton",
        _ => throw new ArgumentOutOfRangeException(nameof(scope))
    };
}

internal record InjectableTypeInfo(
    string ClassName,
    bool IsInterface,
    ImmutableArray<string> Interfaces,
    InjectionScope Scope,
    Platform Platform,
    Location? Location = null,
    bool HasMultipleInterfaces = false);

public enum InjectionScope
{
    Transient,
    Scoped,
    Singleton
}

[Flags]
public enum Platform
{
    None = 0,
    iOS = 1 << 0,
    Android = 1 << 1,
    Windows = 1 << 2,
    MacCatalyst = 1 << 3,
    All = iOS | Android | Windows | MacCatalyst
}