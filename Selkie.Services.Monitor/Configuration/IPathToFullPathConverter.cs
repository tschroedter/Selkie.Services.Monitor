using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    public interface IPathToFullPathConverter
    {
        string ToFullPath([NotNull] string path);
    }
}