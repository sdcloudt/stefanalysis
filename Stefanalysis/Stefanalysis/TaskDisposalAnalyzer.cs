using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Stefanalysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TaskDisposalAnalyzer : DiagnosticAnalyzer
    {
        private const string TaskAssembly = "System.Runtime";
        private const string TaskNamespace = "System.Threading.Tasks";

        public const string DiagnosticId = "SCL0001";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.TaskDisposalAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.TaskDisposalAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.TaskDisposalAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Tasks";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterOperationAction(AnalyzeOperation,
                OperationKind.Invocation);
        }

        private static void AnalyzeOperation(OperationAnalysisContext context)
        {
            IInvocationOperation operation = context.Operation as IInvocationOperation;

            if (operation == null || operation.Instance == null)
            {
                return;
            }

            INamedTypeSymbol type = operation.Instance.Type as INamedTypeSymbol;

            if (type == null)
            {
                return;
            }

            var taskSymbol = context.Compilation.FindNamedTypeSymbol("System.Threading.Tasks.Task");
            
            if (type.IsSubTypeOf(taskSymbol) &&
                operation.TargetMethod.Name == "Dispose")
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    operation.Syntax.GetLocation(),     
                    operation.Instance.Syntax);
                context.ReportDiagnostic(diagnostic);
            }

        }
    }
}
