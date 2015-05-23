using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IServiceProcessCreator
    {
        [CanBeNull]
        ISelkieProcess Create([NotNull] ServiceElement service);
    }
}