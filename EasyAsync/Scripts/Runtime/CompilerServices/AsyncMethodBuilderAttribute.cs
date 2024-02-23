#pragma warning disable SA1633 // File should have header
// from https://github.com/dotnet/csharplang/
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates the type of the async method builder that should be used by a language compiler:
    /// - to build the return type of an async method that is attributed,
    /// - to build the attributed type when used as the return type of an async method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class AsyncMethodBuilderAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="AsyncMethodBuilderAttribute"/> class.</summary>
        /// <param name="builderType">The <see cref="Type"/> of the associated builder.</param>
        public AsyncMethodBuilderAttribute(Type builderType) => this.BuilderType = builderType;

        /// <summary>Gets the <see cref="Type"/> of the associated builder.</summary>
        public Type BuilderType { get; }
    }
}

#pragma warning restore SA1633 // File should have header
