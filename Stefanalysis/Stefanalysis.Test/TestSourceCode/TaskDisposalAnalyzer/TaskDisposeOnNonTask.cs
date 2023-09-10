using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Stefanalysis.Test.TestSourceCode.TaskDisposalAnalyzer
{
    internal class TaskDisposeOnNonTask
    {
        private void Method()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
           cts.Dispose();
        }
    }
}
