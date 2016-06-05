using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class RunningServicesTests
    {
        private static readonly DateTime NotRunningDateTime = DateTime.Parse("2001-01-01 00:00:00");

        [Theory]
        [AutoNSubstituteData]
        public void AddOrUpdateStatusAddsNewServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            // act
            bool actual = runningServices.IsServiceRunning("One");

            // assert
            Assert.True(actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AddOrUpdateStatusUpdatesExistingServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            MakeServiceNotRunning(runningServices,
                                  "One");

            // act
            runningServices.AddOrUpdateStatus("One");

            DateTime actual = runningServices.GetServiceInformation("One").Received;

            // assert
            Assert.True(NotRunningDateTime < actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AreGivenServicesAllRunningReturnsFalseForNotAllRunningTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");
            runningServices.AddOrUpdateStatus("Two");

            MakeServiceNotRunning(runningServices,
                                  "Two");

            string[] given =
            {
                "One",
                "Two"
            };

            // act
            bool actual = runningServices.AreGivenServicesAllRunning(given);

            // assert
            Assert.False(actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void AreGivenServicesAllRunningReturnsTrueForAllRunningTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");
            runningServices.AddOrUpdateStatus("Two");

            string[] given =
            {
                "One",
                "Two"
            };

            // act
            bool actual = runningServices.AreGivenServicesAllRunning(given);

            // assert
            Assert.True(actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CurrentlyRunningReturnsRunningServicesTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");
            runningServices.AddOrUpdateStatus("Two");
            runningServices.AddOrUpdateStatus("Not Running");

            MakeServiceNotRunning(runningServices,
                                  "Not Running");

            // act
            string[] actual = runningServices.CurrentlyRunning().ToArray();

            // assert
            Assert.Equal(2,
                         actual.Length);
            Assert.Equal("One",
                         actual [ 0 ]);
            Assert.Equal("Two",
                         actual [ 1 ]);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetServiceInformationReturnsInformationForKnownServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            // act
            IPingInformation actual = runningServices.GetServiceInformation("One");

            // assert
            Assert.Equal("One",
                         actual.ServiceName);
            Assert.False(actual.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetServiceInformationReturnsInformationForUnknownServiceTest(
            [NotNull] RunningServices runningServices)
        {
            // assemble
            // act
            IPingInformation actual = runningServices.GetServiceInformation("Unknown");

            // assert
            Assert.Equal("Unknown",
                         actual.ServiceName);
            Assert.True(actual.IsUnknown);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetServicesStatusReturnsStringDefaultTest([NotNull] RunningServices runningServices)
        {
            string actual = runningServices.GetStatus();

            Assert.Equal("No service registered!",
                         actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetServicesStatusReturnsStringForOneServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            // act
            string actual = runningServices.GetStatus();
            int numLines = actual.Count(x => x == '\n');

            // assert
            Assert.Equal(1,
                         numLines);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetServicesStatusReturnsStringForTwoServicesTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");
            runningServices.AddOrUpdateStatus("Two");

            // act
            string actual = runningServices.GetStatus();
            int numLines = actual.Count(x => x == '\n');

            // assert
            Assert.Equal(2,
                         numLines);
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsServiceRunningReturnsFalseForNotRunningServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            MakeServiceNotRunning(runningServices,
                                  "One");

            // act
            // assert
            Assert.False(runningServices.IsServiceRunning("One"));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsServiceRunningReturnsFalseForUnknownServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            // act
            // assert
            Assert.False(runningServices.IsServiceRunning("Unknown"));
        }

        [Theory]
        [AutoNSubstituteData]
        public void IsServiceRunningReturnsTrueForRunningServiceTest([NotNull] RunningServices runningServices)
        {
            // assemble
            runningServices.AddOrUpdateStatus("One");

            // act
            // assert
            Assert.True(runningServices.IsServiceRunning("One"));
        }

        private static void MakeServiceNotRunning([NotNull] RunningServices runningServices,
                                                  [NotNull] string serviceName)
        {
            IPingInformation information = runningServices.GetServiceInformation(serviceName);
            information.Received = NotRunningDateTime;
        }
    }
}