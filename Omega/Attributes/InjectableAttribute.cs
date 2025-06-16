namespace Omega.Attributes;

/// <summary>
/// Specifies the injection scope and platform for a service.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class InjectableAttribute : Attribute
{
    /// <summary>
    /// Gets the injection scope.
    /// </summary>
    public InjectionScope Scope { get; }

    /// <summary>
    /// Gets the target platform.
    /// </summary>
    public Platform Platform { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InjectableAttribute"/> class.
    /// </summary>
    /// <param name="scope">The injection scope.</param>
    /// <param name="platform">The target platform.</param>
    public InjectableAttribute(InjectionScope scope, Platform platform)
    {
        Scope = scope;
        Platform = platform;
    }
}

/// <summary>
/// Defines the injection scope for a service.
/// </summary>
public enum InjectionScope
{
    /// <summary>
    /// A new instance is created for each request.
    /// </summary>
    Transient,

    /// <summary>
    /// A single instance is created per scope.
    /// </summary>
    Scoped,

    /// <summary>
    /// A single instance is created for the entire application lifetime.
    /// </summary>
    Singleton
}

/// <summary>
/// Defines the target platform for a service.
/// </summary>
[Flags]
public enum Platform
{
    /// <summary>
    /// No specific platform.
    /// </summary>
    None = 0,

    /// <summary>
    /// iOS platform.
    /// </summary>
    iOS = 1 << 0,

    /// <summary>
    /// Android platform.
    /// </summary>
    Android = 1 << 1,

    /// <summary>
    /// Windows platform.
    /// </summary>
    Windows = 1 << 2,

    /// <summary>
    /// macOS platform.
    /// </summary>
    MacCatalyst = 1 << 3,

    /// <summary>
    /// All platforms.
    /// </summary>
    All = iOS | Android | Windows | MacCatalyst
} 