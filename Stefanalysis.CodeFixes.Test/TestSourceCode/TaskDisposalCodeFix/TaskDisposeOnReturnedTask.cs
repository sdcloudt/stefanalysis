using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class TaskDisposeOnReturnedTask
    {
        private Task CreateTask()
        {
            return new Task(() => { });
        }

        private void Method()
        {
        }
    }
}
