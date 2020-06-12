using System;
using System.IO;

namespace Skimur.IO
{
    public interface IFileInfo
    {
        bool Exists { get; }

        void Open(FileMode mode, Action<Stream> action);

        Stream Open(FileMode mode);

        void Delete();
    }
}
