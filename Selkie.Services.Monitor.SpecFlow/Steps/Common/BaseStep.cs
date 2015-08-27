using System;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using TechTalk.SpecFlow;

namespace Selkie.Services.Monitor.SpecFlow.Steps.Common
{
    [Binding]
    public abstract class BaseStep
    {
        private readonly ISelkieBus m_Bus;
        private readonly StepHelper m_Helper = new StepHelper();

        protected BaseStep()
        {
            m_Bus = ( ISelkieBus ) ScenarioContext.Current [ "ISelkieBus" ];
        }

        protected ISelkieBus Bus
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