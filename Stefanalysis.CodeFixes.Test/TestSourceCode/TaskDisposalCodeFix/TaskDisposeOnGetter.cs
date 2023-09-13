using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class TaskDisposeOnGetter
    {
        private Task TaskProperty => Task.Delay(10);

        public void SomeMethod()
        {
        }
    }
}
