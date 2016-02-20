using JetBrains.Annotations;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathToFullPathConverter : IPathToFullPathConverter
    {
        private readonly ISelkieEnvironment m_Environment;

        public PathToFullPathConverter([NotNull] ISelkieEnvironment environment)
        {
            m_Environment = environment;
        }

        public string ToFullPath(string path)
        {
            var fullPath = path;

            if (fullPath.StartsWith(".\\"))
            {
                fullPath = m_Environment.CurrentDirectory + fullPath.Substring(1);
            }

            fullPath = fullPath.Replace("//",
                                        "\\");

            if ( !fullPath.EndsWith("\\") )
            {
                fullPath += "\\";
            }

            return fullPath;
        }
    }
}