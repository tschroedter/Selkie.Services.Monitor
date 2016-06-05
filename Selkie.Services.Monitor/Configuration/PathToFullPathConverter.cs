using JetBrains.Annotations;
using Selkie.Common.Interfaces;
using Selkie.Windsor;

namespace Selkie.Services.Monitor.Configuration
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathToFullPathConverter : IPathToFullPathConverter
    {
        public PathToFullPathConverter([NotNull] ISelkieEnvironment environment)
        {
            m_Environment = environment;
        }

        private readonly ISelkieEnvironment m_Environment;

        public string ToFullPath(string path)
        {
            string fullPath = path;

            if ( fullPath.StartsWith(".\\") )
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