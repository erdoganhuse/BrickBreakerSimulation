using JetBrains.Annotations;

namespace Library.DependencyInjection
{
    [System.AttributeUsage( System.AttributeTargets.Field )]
    [MeansImplicitUse]
    public sealed class Inject : System.Attribute { }

    public class InjectorException : System.Exception
    {
        public InjectorException( string message ) : base( message ) {  }
    }
}