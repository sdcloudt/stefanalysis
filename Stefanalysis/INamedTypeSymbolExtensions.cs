using Microsoft.CodeAnalysis;

namespace Stefanalysis
{
    public static class INamedTypeSymbolExtensions
    {
        public static bool IsSubTypeOf(this INamedTypeSymbol symbol, ISymbol parent) 
        {
            if (SymbolEqualityComparer.Default.Equals(symbol, parent))
            {
                return true;
            }

            if (symbol.BaseType != null)
            {
                return symbol.BaseType.IsSubTypeOf(parent);
            }

            return false;
        }
    }
}
