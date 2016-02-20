using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Monitor.Configuration;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class PathToFullPathConverterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void Create_ReturnsFullPath_ForFullPathEndingWithSlash([NotNull] PathToFullPathConverter sut)
        {
            // assemble
            var expected = @"C:\Temp\";

            // act
            string actual = sut.ToFullPath(@"C:\Temp\");

            // assert
            Assert.Equal(expected,
                         actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_ReturnsFullPathEndingWithSlash_ForFullPath([NotNull] PathToFullPathConverter sut)
        {
            // assemble
            var expected = @"C:\Temp\";

            // act
            string actual = sut.ToFullPath(@"C:\Temp");

            // assert
            Assert.Equal(expected,
                         actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_ReturnsFullPath_ForRelativePath([NotNull, Frozen] ISelkieEnvironment environment,
                                                           [NotNull] PathToFullPathConverter sut)
        {
            // assemble
            environment.CurrentDirectory.Returns(@"C:");
            var expected = @"C:\Temp\";

            // act
            string actual = sut.ToFullPath(@".\Temp");

            // assert
            Assert.Equal(expected,
                         actual);
        }
    }
}