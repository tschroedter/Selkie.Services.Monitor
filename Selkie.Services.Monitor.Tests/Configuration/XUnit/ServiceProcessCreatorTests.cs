using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Selkie.Services.Monitor.Configuration;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Monitor.Tests.Configuration.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class ServiceProcessCreatorTests
    {
        // todo try to get sut into AutoNSub...
        [Theory]
        [AutoNSubstituteData]
        public void Create_ReturnsProcess_WhenCalled([NotNull] ISelkieLogger logger,
                                                     [NotNull] ISelkieProcess selkieProcess,
                                                     [NotNull] IPathToFullPathConverter converter,
                                                     [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var factory = Substitute.For <ISelkieProcessFactory>();
            ISelkieProcess process = factory.Create(Arg.Any <ProcessStartInfo>());
            process.Returns(selkieProcess);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            ISelkieProcess actual = creator.Create(serviceElement);

            // assert
            Assert.Equal(selkieProcess,
                         actual);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_CallToFullPathForFolderName_WhenCalled([NotNull] ISelkieLogger logger,
                                                                  [NotNull] ISelkieProcess selkieProcess,
                                                                  [NotNull] IPathToFullPathConverter converter,
                                                                  [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var factory = Substitute.For <ISelkieProcessFactory>();
            ISelkieProcess process = factory.Create(Arg.Any <ProcessStartInfo>());
            process.Returns(selkieProcess);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            creator.Create(serviceElement);

            // assert
            converter.Received().ToFullPath(serviceElement.FolderName);
        }


        [Theory]
        [AutoNSubstituteData]
        public void Create_CallToFullPathForWorkingFolder_WhenCalled([NotNull] ISelkieLogger logger,
                                                                     [NotNull] ISelkieProcess selkieProcess,
                                                                     [NotNull] IPathToFullPathConverter converter,
                                                                     [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var factory = Substitute.For <ISelkieProcessFactory>();
            ISelkieProcess process = factory.Create(Arg.Any <ProcessStartInfo>());
            process.Returns(selkieProcess);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            creator.Create(serviceElement);

            // assert
            converter.Received().ToFullPath(serviceElement.WorkingFolder);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_CallToFullPathForWorkingName_WhenCalled([NotNull] ISelkieLogger logger,
                                                                   [NotNull] ISelkieProcess selkieProcess,
                                                                   [NotNull] IPathToFullPathConverter converter,
                                                                   [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var factory = Substitute.For <ISelkieProcessFactory>();
            ISelkieProcess process = factory.Create(Arg.Any <ProcessStartInfo>());
            process.Returns(selkieProcess);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            creator.Create(serviceElement);

            // assert
            converter.Received().ToFullPath(serviceElement.WorkingFolder);
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_CallsToFullPathForFolderName_WhenCalled([NotNull] ISelkieLogger logger,
                                                                   [NotNull] ISelkieProcessFactory factory,
                                                                   [NotNull] IPathToFullPathConverter converter,
                                                                   [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var expected = "C:\\Temp";
            serviceElement.FolderName = ".\\Temp";
            converter.ToFullPath(serviceElement.FolderName).Returns(expected);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            creator.Create(serviceElement);

            // assert
            factory.Received()
                   .Create(Arg.Is <ProcessStartInfo>(x => x.Arguments.Contains(expected)));
        }

        [Theory]
        [AutoNSubstituteData]
        public void Create_CallsToFullPathForWorkingFolder_WhenCalled([NotNull] ISelkieLogger logger,
                                                                      [NotNull] ISelkieProcessFactory factory,
                                                                      [NotNull] IPathToFullPathConverter converter,
                                                                      [NotNull] ServiceElement serviceElement)
        {
            // assemble
            var expected = "C:\\Temp";
            serviceElement.WorkingFolder = "C:\\Temp";
            converter.ToFullPath(serviceElement.WorkingFolder).Returns(expected);

            var creator = new ServiceProcessCreator(logger,
                                                    factory,
                                                    converter);

            // act
            creator.Create(serviceElement);

            // assert
            factory.Received()
                   .Create(Arg.Is <ProcessStartInfo>(x => x.WorkingDirectory == expected));
        }
    }
}