using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Common;
using Selkie.Services.Common.Messages;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    //ncrunch: no coverage start
    // ReSharper disable once ClassTooBig
    [ExcludeFromCodeCoverage]
    public sealed class ServicesMonitorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void GetServicesStatusCallsMonitorTest([NotNull] [Frozen] IRunningServices runningServices,
                                                      [NotNull] ServicesMonitor monitor)
        {
            monitor.GetServicesStatus();

            runningServices.Received().GetStatus();
        }

        [Fact]
        public void StartCallsInitializeOnTimerStartServices()
        {
            var timer = Substitute.For <ITimer>();

            var monitor = new ServicesMonitor(Substitute.For <IBus>(),
                                              Substitute.For <ILogger>(),
                                              Substitute.For <IRunningServices>(),
                                              Substitute.For <INotRunningServices>(),
                                              timer);

            monitor.Start();

            timer.Received().Initialize(monitor.OnTimer,
                                        ServicesMonitor.FiveSeconds,
                                        ServicesMonitor.TwoSeconds);
        }

        [Theory]
        [AutoNSubstituteData]
        public void PingCallsWriteLineTest([NotNull] [Frozen] ILogger logger,
                                           [NotNull] ServicesMonitor monitor)
        {
            monitor.Ping();

            logger.Received().Debug(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void PingSendMessageToLineServiceTest([NotNull] [Frozen] IBus bus,
                                                     [NotNull] ServicesMonitor monitor)
        {
            monitor.Ping();

            bus.Received().PublishAsync(Arg.Any <PingRequestMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerLogStatusCallsGetServicesStatusTest([NotNull] [Frozen] IRunningServices runningServices,
                                                               [NotNull] ServicesMonitor monitor)
        {
            monitor.LogStatus();

            runningServices.Received().GetStatus();
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerLogStatusCallsWriteLineTest([NotNull] [Frozen] ILogger logger,
                                                       [NotNull] [Frozen] IRunningServices runningServices,
                                                       [NotNull] ServicesMonitor monitor)
        {
            runningServices.GetStatus().Returns("Test");

            monitor.LogStatus();

            logger.Received().Info(Arg.Is <string>(x => x == "Test"));
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerServicesCallsPingTest([NotNull] [Frozen] IBus bus,
                                                 [NotNull] [Frozen] INotRunningServices notRunningServices,
                                                 [NotNull] ServicesMonitor monitor)
        {
            notRunningServices.AreAllServicesRunning().Returns(false);

            monitor.OnTimer(this);

            bus.Received().PublishAsync(Arg.Any <PingRequestMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerSendsStatusMessageWhenAllServicesRunningTest(
            [NotNull] [Frozen] INotRunningServices notRunningServices,
            [NotNull] [Frozen] IBus bus,
            [NotNull] ServicesMonitor monitor)
        {
            // assemble
            notRunningServices.AreAllServicesRunning().Returns(true);

            // act
            monitor.OnTimer(this);

            // assert
            bus.Received().PublishAsync(Arg.Any <ServicesStatusResponseMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerSendsServicesStatusResponseMessageTest([NotNull] [Frozen] IBus bus,
                                                                  [NotNull] ServicesMonitor monitor)
        {
            // assemble
            // act
            monitor.OnTimer(this);

            // assert
            bus.Received().PublishAsync(Arg.Any <ServicesStatusResponseMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerWritesToLogTest([NotNull] [Frozen] ILogger logger,
                                           [NotNull] ServicesMonitor monitor)
        {
            // assemble
            // act
            monitor.OnTimer(this);

            // assert
            logger.Received().Info(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void OnTimerSendsPingRequestMessageTest([NotNull] [Frozen] IBus bus,
                                                       [NotNull] ServicesMonitor monitor)
        {
            // assemble
            // act
            monitor.OnTimer(this);

            // assert
            bus.Received().PublishAsync(Arg.Any <PingRequestMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsServiceRunningCallsPingResponseMonitorTest([NotNull] [Frozen] IRunningServices runningServices,
                                                                 [NotNull] ServicesMonitor monitor)
        {
            monitor.IsServiceRunning("TestService");

            runningServices.Received().IsServiceRunning(Arg.Is <string>(x => x == "TestService"));
        }

        [Theory]
        [AutoNSubstituteData]
        public void SendStatusMessageSendMessageTest([NotNull] [Frozen] IBus bus,
                                                     [NotNull] [Frozen] INotRunningServices notRunningServices,
                                                     [NotNull] ServicesMonitor monitor)
        {
            // assemble
            notRunningServices.AreAllServicesRunning().Returns(true);

            // act
            monitor.SendStatusMessage();

            // assert
            bus.Received().PublishAsync(Arg.Is <ServicesStatusResponseMessage>(x => x.IsAllServicesRunning));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                         [NotNull] ServicesMonitor monitor)
        {
            // assemble
            // act
            monitor.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                        [NotNull] ServicesMonitor monitor)
        {
            // assemble
            // act
            monitor.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }
    }
}