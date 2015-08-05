using System;
using EasyNetQ;
using JetBrains.Annotations;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps.Common
{
    [Binding]
    public abstract class BaseStep
    {
        private readonly IBus m_Bus;
        private readonly StepHelper m_Helper = new StepHelper();

        protected BaseStep()
        {
            m_Bus = ( IBus ) ScenarioContext.Current [ "IBus" ];
        }

        protected IBus Bus
        {
            get
            {
                return m_Bus;
            }
        }

        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            m_Helper.SleepWaitAndDo(breakIfTrue,
                                    doSomething);
        }

        public void DoNothing()
        {
        }

        public abstract void Do();
    }
}