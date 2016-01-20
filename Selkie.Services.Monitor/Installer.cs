using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Selkie.Common;
using Selkie.Services.Common;
using Selkie.Services.Monitor.Configuration;

namespace Selkie.Services.Monitor
{
    //ncrunch: no coverage start
    [ExcludeFromCodeCoverage]
    public class Installer : SelkieInstaller <Installer>
    {
        protected override void InstallComponents(IWindsorContainer container,
                                                  IConfigurationStore store)
        {
            base.InstallComponents(container,
                                   store);

            // ReSharper disable MaximumChainedReferences
            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <IService>()
                                      .WithServiceFromInterface(typeof ( IService ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)),
                               Component.For <ISelkieProcessFactory>().AsFactory());
            // ReSharper restore MaximumChainedReferences
        }
    }
}