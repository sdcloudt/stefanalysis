using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stefanalysis
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TaskDisposalCodeFixProvider)), Shared]
    public class TaskDisposalCodeFixProvider: CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(TaskDisposalAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var expression = root
                .FindToken(diagnosticSpan.Start)
                .Parent
                .AncestorsAndSelf()
                .OfType<ExpressionStatementSyntax>()
                .First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.TaskDisposalCodeFixTitle,
                    createChangedDocument: c => RemoveExpression(context.Document, expression, c),
                    equivalenceKey: nameof(CodeFixResources.TaskDisposalCodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> RemoveExpression(Document document, ExpressionStatementSyntax expression, CancellationToken token)
        {
            SyntaxNode oldRoot = await document.GetSyntaxRootAsync (token).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot.RemoveNode(expression, SyntaxRemoveOptions.KeepNoTrivia);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
