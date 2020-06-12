using System;
using System.IO;

namespace Skimur.IO
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly string _baseDirectory;

        public LocalFileSystem(string baseDirectory)
        {
            _baseDirectory = baseDirectory;

            // check if the directory exists, if not create it
            if (!Directory.Exists(_baseDirectory))
            {
                // create the directory
                Directory.CreateDirectory(_baseDirectory);
            }
        }


        public IDirectoryInfo GetDirectory(string directory)
        {
            return new LocalDirectoryInfo(Path.Combine(_baseDirectory, directory), directory);
        }

        public IFileInfo GetFile(string file)
        {
            return new LocalFileInfo(Path.Combine(_baseDirectory, file), file);
        }

        internal class LocalFileInfo : IFileInfo
        {
            private readonly string _localPath;
            private readonly string _relativePath;

            public LocalFileInfo(string localPath, string relativePath)
            {
                _localPath = localPath;
                _relativePath = relativePath;
            }

            public bool Exists
            {
                get
                {
                    return File.Exists(_localPath);
                }
            }

            public void Open(FileMode mode, Action<Stream> action)
            {
                using (var stream = File.Open(_localPath, mode))
                {
                    action(stream);
                }
            }

            public Stream Open(FileMode mode)
            {
                return File.Open(_localPath, mode);
            }

            public void Delete()
            {
                File.Delete(_localPath);
            }
        }

        internal class LocalDirectoryInfo : IDirectoryInfo
        {
            private readonly string _localPath;
            private readonly string _relativePath;

            public LocalDirectoryInfo(string localPath, string relativePath)
            {
                _localPath = localPath;
                _relativePath = relativePath;
            }

            public string Path
            {
                get
                {
                    return _relativePath;
                }
            }

            public bool Exists
            {
                get
                {
                    return Directory.Exists(_localPath);
                }
            }

            public void Create()
            {
                Directory.CreateDirectory(_localPath);
            }

            public void Delete()
            {
                Directory.Delete(_localPath);
            }

            public void Delete(bool recursive)
            {
                Directory.Delete(_localPath, recursive);
            }

            public IFileInfo GetFile(string file)
            {
                return new LocalFileInfo(System.IO.Path.Combine(_localPath, file), System.IO.Path.Combine(_relativePath, file));
            }

            public bool FileExists(string file)
            {
                return File.Exists(System.IO.Path.Combine(_localPath, file));
            }

            public void DeleteFile(string file)
            {
                File.Delete(System.IO.Path.Combine(_localPath, file));
            }
        }

    }
}
