using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class TaskDisposeOnVarVariable
    {
        public void Method()
        {
            var delayTask = Task.Delay(500);
        }
    }
}
