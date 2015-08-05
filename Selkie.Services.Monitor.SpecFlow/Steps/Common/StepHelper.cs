using System;
using System.Threading;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor.SpecFlow.Steps.Common
{
    public sealed class StepHelper
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(2.0);

        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            for ( var i = 0 ; i < 10 ; i++ )
            {
                Thread.Sleep(SleepTime);

                if ( breakIfTrue() )
                {
                    break;
                }

                doSomething();
            }
        }
    }
}