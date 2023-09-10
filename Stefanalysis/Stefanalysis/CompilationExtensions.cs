using Microsoft.CodeAnalysis;
using System.Linq;

namespace Stefanalysis
{
    public static class CompilationExtensions
    {
        public static INamedTypeSymbol FindNamedTypeSymbol(this Compilation compilation, string canonicalName)
        {
            var candidates = compilation
                .SourceModule
                .ReferencedAssemblySymbols
                .Select(ass => ass.GetTypeByMetadataName(canonicalName))
                .Where(ass => ass != null)
                .ToList();

            if (candidates.Count != 1)
            {
                return null;
            }

            return candidates[0];
        }
    }
}
