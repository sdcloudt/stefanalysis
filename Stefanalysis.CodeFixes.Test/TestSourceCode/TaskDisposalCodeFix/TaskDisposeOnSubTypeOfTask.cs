using System;
using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class SubTask : Task
    {
        public SubTask(Action action) : base(action)
        {
        }
    }

    internal class TaskDisposeOnSubTypeOfTask
    {
        private void Method()
        {
            var subtask = new SubTask(() => { });
        }
    }
}
