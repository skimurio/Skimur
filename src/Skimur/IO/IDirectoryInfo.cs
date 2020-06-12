using System;
namespace Skimur.IO
{
    public interface IDirectoryInfo
    {
        string Path { get; }

        bool Exists { get; }

        void Create();

        void Delete();

        void Delete(bool recursive);

        IFileInfo GetFile(string file);

        bool FileExists(string file);

        void DeleteFile(string file);
    }
}
