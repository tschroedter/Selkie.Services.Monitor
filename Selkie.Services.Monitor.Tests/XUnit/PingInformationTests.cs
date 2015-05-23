using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class PingInformationTests
    {
        [Fact]
        public void UnknownTest()
        {
            Assert.True(PingInformation.Unknown.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void KnownTest([NotNull] PingInformation pingInformation)
        {
            Assert.False(pingInformation.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void ServiceNameRoundtripTest([NotNull] PingInformation pingInformation)
        {
            pingInformation.ServiceName = "One";

            Assert.Equal("One",
                         pingInformation.ServiceName);
        }

        [Theory]
        [AutoNSubstituteData]
        public void ReceivedRoundtripTest([NotNull] PingInformation pingInformation)
        {
            DateTime received = DateTime.Parse("2001-01-01 00:00:00");

            pingInformation.Received = received;

            Assert.True(received == pingInformation.Received);
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsRunningReturnsTrueForRunningServiceTest([NotNull] PingInformation pingInformation)
        {
            DateTime received = DateTime.Now;

            pingInformation.Received = received;

            Assert.True(pingInformation.IsRunning);
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsRunningReturnsTrueForNotRunningServiceTest([NotNull] PingInformation pingInformation)
        {
            DateTime received = DateTime.Now.Subtract(PingInformation.ExpiredAge);

            pingInformation.Received = received;

            Assert.False(pingInformation.IsRunning);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AgeOfPingTest([NotNull] PingInformation pingInformation)
        {
            DateTime received = DateTime.Now.Subtract(TimeSpan.FromSeconds(2));
            pingInformation.Received = received;

            TimeSpan ageOfPing = pingInformation.AgeOfPing;

            Assert.True(2 == ageOfPing.Seconds);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AgeInMillisecondsForRunningTest([NotNull] PingInformation pingInformation)
        {
            DateTime received = DateTime.Now.Subtract(TimeSpan.FromSeconds(2));
            pingInformation.Received = received;

            double actual = pingInformation.AgeInMilliseconds;

            Assert.True(actual > 0.0);
        }
    }

    //ncrunch: no coverage start
}