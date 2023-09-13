using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class TaskDisposeOnTaskWithTypeParameter
    {
        public void Method()
        {
        }

        private Task<int> GetTask()
        {
            return Task.FromResult(0);
        }
    }
}
