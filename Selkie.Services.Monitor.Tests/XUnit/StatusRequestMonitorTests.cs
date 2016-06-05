using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Monitor.Common.Messages;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class StatusRequestMonitorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] StatusRequestMonitor monitor)
        {
            // assemble
            // act
            monitor.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        // ReSharper disable once TooManyArguments
        public void StatusRequestHandlerSendsMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                         [NotNull] [Frozen] INotRunningServices notRunningServices,
                                                         [NotNull] [Frozen] IRunningServices runningServices,
                                                         [NotNull] StatusRequestMonitor requestMonitor)
        {
            // assemble
            notRunningServices.AreAllServicesRunning().Returns(true);
            runningServices.CurrentlyRunning().Returns(new[]
                                                       {
                                                           "One"
                                                       });

            var message = new StatusRequestMessage();

            // act
            requestMonitor.StatusRequestHandler(message);

            // assert
            bus.Received()
               .PublishAsync(
                             Arg.Is <StatusResponseMessage>(
                                                            x =>
                                                            x.AreAllServicesRunning &&
                                                            x.RunningServices.First() == "One" &&
                                                            !x.NotRunningServices.Any()));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] StatusRequestMonitor monitor)
        {
            // assemble
            // act
            monitor.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToStatusRequestMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                         [NotNull] StatusRequestMonitor requestMonitor)
        {
            // assemble
            string subscriptionId = requestMonitor.GetType().ToString();

            // act
            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <StatusRequestMessage>>());
        }
    }
}