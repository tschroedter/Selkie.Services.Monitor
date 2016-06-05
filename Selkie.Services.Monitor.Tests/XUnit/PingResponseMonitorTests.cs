using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    // ReSharper disable once ClassTooBig
    public sealed class PingResponseMonitorTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void HandlePingResponseCallsAddOrUpdateStatusTest([NotNull] [Frozen] IRunningServices runningServices,
                                                                 [NotNull] PingResponseMonitor monitor)
        {
            // assemble
            PingResponseMessage message = CreatePingResponseMessageForTwo();

            // act
            monitor.PingResponseHandler(message);

            // assert
            runningServices.Received().AddOrUpdateStatus(message.ServiceName);
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsMessageIgnoredReturnsFalseForOtherServiceTest([NotNull] PingResponseMonitor monitor)
        {
            var message = new PingResponseMessage
                          {
                              ServiceName = "Other Service Name",
                              Request = DateTime.Now
                          };

            Assert.False(monitor.IsMessageIgnored(message));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsMessageIgnoredReturnsTrueForServiceMonitorTest([NotNull] PingResponseMonitor monitor)
        {
            var message = new PingResponseMessage
                          {
                              ServiceName = Service.ServiceName,
                              Request = DateTime.Now
                          };

            Assert.True(monitor.IsMessageIgnored(message));
        }

        [Theory]
        [AutoNSubstituteData]
        public void PingResponseHandlerCallsWriteLineTest([NotNull] [Frozen] ISelkieLogger logger,
                                                          [NotNull] PingResponseMonitor monitor,
                                                          [NotNull] PingResponseMessage message)
        {
            // act
            monitor.PingResponseHandler(message);

            // assert
            logger.Received().Debug(Arg.Is <string>(x => x.Contains(message.ServiceName)));
        }

        [Theory]
        [AutoNSubstituteData]
        public void PingResponseHandlerIgnoresMessageFromServiceMonitorTest(
            [NotNull] [Frozen] IRunningServices runningServices,
            [NotNull] PingResponseMonitor monitor)
        {
            // assemble
            var message = new PingResponseMessage
                          {
                              ServiceName = Service.ServiceName,
                              Request = DateTime.Now
                          };

            // act
            monitor.PingResponseHandler(message);

            // assert
            runningServices.DidNotReceive().AddOrUpdateStatus(message.ServiceName);
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] PingResponseMonitor monitor)
        {
            // assemble
            // act
            monitor.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] PingResponseMonitor monitor)
        {
            // assemble
            // act
            monitor.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToPingResponseMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                        [NotNull] PingResponseMonitor monitor)
        {
            // assemble
            string subscriptionId = monitor.GetType().ToString();

            // act
            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <PingResponseMessage>>());
        }

        [NotNull]
        private static PingResponseMessage CreatePingResponseMessageForTwo()
        {
            var message = new PingResponseMessage
                          {
                              ServiceName = "Two",
                              Request = DateTime.Now
                          };
            return message;
        }
    }
}