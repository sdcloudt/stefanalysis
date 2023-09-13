using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class SimpleDisposeCall
    {
        private void Method()
        {
            Task task = new Task(() => { });
        }
    }
}
