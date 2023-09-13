using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerifyCS = Stefanalysis.Test.CSharpAnalyzerVerifier<Stefanalysis.TaskDisposalAnalyzer>;

namespace Stefanalysis.Test
{
    [TestClass]
    public class TaskDisposalAnalyzerTest
    {
        private const string TestRootPath = @"TestSourceCode\TaskDisposalAnalyzer";

        private string DiagnosticId => "SCL0001";

        public class DiagnosticData
        {
            public int Line { get; set; }
            public int Column { get; set; }
            public string Variable { get; set; }
        }

        public static IList<object[]> TestData => new List<object[]>()
        {
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnArgument.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 12, Column = 13, Variable = "task"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnGetter.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 11, Column = 13, Variable = "TaskProperty"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnNonTask.cs",
                new DiagnosticData[0]
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnReturnedTask.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 17, Column = 13, Variable = "CreateTask()"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnSubTypeOfTask.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 18, Column = 13, Variable = "subtask"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnTaskWithTypeParameter.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 9, Column = 13, Variable = "GetTask()"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnVariable.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 10, Column = 13,
                        Variable = "task"
                    }
                }
            },
            new object[]
            {
                TestRootPath + @"\TaskDisposeOnVarVariable.cs",
                new DiagnosticData[]
                {
                    new DiagnosticData()
                    {
                        Line = 10, Column = 13, Variable = "delayTask"
                    }
                }
            }
        };

        [TestMethod]
        [DynamicData(nameof(TestData))]
        public async Task TestTaskDisposalAnalyzer(string filepath, DiagnosticData[] testData)
        {
            var expected = testData.Select(x =>
                VerifyCS.Diagnostic(DiagnosticId)
                    .WithLocation(filepath, x.Line, x.Column)
                    .WithArguments(x.Variable)).ToArray();

            await VerifyCS.VerifyAnalyzerAsync(filepath, expected);
        }
    }
}
