using System;

namespace Skimur.IO
{
    public interface IFileSystem
    {
        IDirectoryInfo GetDirectory(string directory);

        IFileInfo GetFile(string file);
    }
}
