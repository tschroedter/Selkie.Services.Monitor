using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IServiceStarter
    {
        [NotNull]
        ISelkieProcess Start([NotNull] ServiceElement serviceElement);
    }
}