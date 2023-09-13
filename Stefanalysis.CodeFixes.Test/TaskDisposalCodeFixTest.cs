using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stefanalysis.Test;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerifyCS = Stefanalysis.Test.CSharpCodeFixVerifier<Stefanalysis.TaskDisposalAnalyzer, Stefanalysis.TaskDisposalCodeFixProvider>;

namespace Stefanalysis.CodeFixes.Test
{
    [TestClass]
    public class TaskDisposalCodeFixTest
    {
        private const string SourceRootPath = @"TestSourceCode\TaskDisposalAnalyzer";
        private const string FixedSourceRootPath = @"TestSourceCode\TaskDisposalCodeFix";
        private string DiagnosticId => "SCL0001";

        public static IList<object[]> TestData => new List<object[]>()
        {
            new object[]
            {
                @"\TaskDisposeOnArgument.cs",
                TaskDisposalAnalyzerTest.TestData[0][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnGetter.cs",
                TaskDisposalAnalyzerTest.TestData[1][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnReturnedTask.cs",
                TaskDisposalAnalyzerTest.TestData[3][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnSubTypeOfTask.cs",
                TaskDisposalAnalyzerTest.TestData[4][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnTaskWithTypeParameter.cs",
                TaskDisposalAnalyzerTest.TestData[5][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnVariable.cs",
                TaskDisposalAnalyzerTest.TestData[6][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            },
            new object[]
            {
                @"\TaskDisposeOnVarVariable.cs",
                TaskDisposalAnalyzerTest.TestData[7][1] as TaskDisposalAnalyzerTest.DiagnosticData[]
            }
        };

        [TestMethod]
        [DynamicData(nameof(TestData))]
        public async Task TestCodeFixer(string fileName, TaskDisposalAnalyzerTest.DiagnosticData[] testData)
        {
            string sourcePath = SourceRootPath + fileName;
            string fixedSourcePath = FixedSourceRootPath + fileName;

            var expected = testData.Select(x =>
                VerifyCS.Diagnostic(DiagnosticId)
                    .WithLocation(sourcePath, x.Line, x.Column)
                    .WithArguments(x.Variable)).ToArray();

            await VerifyCS.VerifyCodeFixAsync(sourcePath, expected, fixedSourcePath);
        }
    }
}
