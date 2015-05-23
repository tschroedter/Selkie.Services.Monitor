using System.Diagnostics;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface ISelkieProcessFactory
    {
        [NotNull]
        ISelkieProcess Create([NotNull] ProcessStartInfo processStartInfo);

        void Release([NotNull] ISelkieProcess selkieProcess);
    }
}