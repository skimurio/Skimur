using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace Skimur.IO
{
    public class PathResolver : IPathResolver
    {
        public string Resolve(string path)
        {
            var appEnv = PlatformServices.Default.Application;

            if (Path.IsPathRooted(path))
            {
                return path;
            }

            if (path.StartsWith("~" + Path.DirectorySeparatorChar))
            {
                return Path.GetFullPath(Path.Combine(appEnv.ApplicationBasePath, path.Substring(2)));
            }

            return Path.GetFullPath(Path.Combine(appEnv.ApplicationBasePath, path));
        }
    }
}
