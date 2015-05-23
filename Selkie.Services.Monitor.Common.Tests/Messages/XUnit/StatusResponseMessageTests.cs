using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Selkie.Services.Monitor.Common.Messages;
using Xunit;

namespace Selkie.Services.Monitor.Common.Tests.Messages.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class StatusResponseMessageTests
    {
        [Fact]
        public void AreAllServicesRunningRoundtripTest()
        {
            // assemble
            var message = new StatusResponseMessage();
            Assert.False(message.AreAllServicesRunning);

            // act
            message.AreAllServicesRunning = true;

            // assert
            Assert.True(message.AreAllServicesRunning);
        }

        [Fact]
        public void RunningServicesRoundtripTest()
        {
            // assemble
            // act
            var message = new StatusResponseMessage
                          {
                              RunningServices = new[]
                                                {
                                                    "One",
                                                    "Two"
                                                }
                          };

            // assert
            Assert.Equal(2,
                         message.RunningServices.Count());
        }

        [Fact]
        public void NotRunningServicesRoundtripTest()
        {
            // assemble
            // act
            var message = new StatusResponseMessage
                          {
                              NotRunningServices = new[]
                                                   {
                                                       "One",
                                                       "Two"
                                                   }
                          };

            // assert
            Assert.Equal(2,
                         message.NotRunningServices.Count());
        }
    }
}