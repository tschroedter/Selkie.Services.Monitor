using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using Selkie.Services.Monitor.Configuration;
using Xunit;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServicesConfigurationRepositoryTests
    {
        [Fact]
        public void Constructor_CreatesEmptyRepository_WhenSectionDoesNotExist()
        {
            // assemble
            ServicesConfigurationRepository sut = CreateSutWithNoSection();

            // act
            IEnumerable <ServiceElement> actual = sut.GetAll();

            // assert
            Assert.Equal(0,
                         actual.Count());
        }

        [Fact]
        public void Constructor_CreatesRepository_WhenSectionExists()
        {
            // assemble
            ServicesConfigurationRepository sut = CreateSut();

            // act
            IEnumerable <ServiceElement> actual = sut.GetAll();

            // assert
            Assert.Equal(3,
                         actual.Count());
        }

        [Fact]
        public void GetByServiceName_RetrunsUnknown_ForUnknownName()
        {
            // assemble
            ServicesConfigurationRepository sut = CreateSut();

            // act
            // assert
            Assert.Equal(ServiceElement.Unknown,
                         sut.GetByServiceName("Unknown"));
        }

        [Fact]
        public void GetByServiceName_RetrunsKnown_ForName()
        {
            // assemble
            ServicesConfigurationRepository sut = CreateSut();

            // act
            ServiceElement actual = sut.GetByServiceName("Aco Service");

            // assert
            Assert.Equal("Aco Service",
                         actual.ServiceName);
        }

        private ServicesConfigurationRepository CreateSut()
        {
            var manager = new SelkieConfigurationManager();
            var sut = new ServicesConfigurationRepository(manager);
            return sut;
        }

        private ServicesConfigurationRepository CreateSutWithNoSection()
        {
            var manager = Substitute.For <ISelkieConfigurationManager>();
            manager.GetSection("services").Returns(( IServicesConfigurationSection ) null);

            var sut = new ServicesConfigurationRepository(manager);
            return sut;
        }
    }
}