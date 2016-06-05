using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IServiceElementStarter
    {
        void StartService(
            [NotNull] ServiceElement serviceElement);
    }
}