using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using Skimur.IO;
using Skimur.Web.Utils;

namespace Skimur.Web.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystemProvider;
        private readonly IDirectoryInfo _avatarDirectoryInfo;

        public AvatarService(IPathResolver pathResolver, IFileSystem fileSystemProvider)
        {
            _pathResolver = pathResolver;
            _fileSystemProvider = fileSystemProvider;

            _avatarDirectoryInfo = _fileSystemProvider.GetDirectory("avatars");
            if (!_avatarDirectoryInfo.Exists)
            {
                _avatarDirectoryInfo.Create();
            }

        }

        public string UploadAvatar(IFormFile file, string key)
        {
            if (file.Length >= 300000)
            {
                throw new Exception("Uploaded image may not exceed 300 kb, please upload a smaller image.");
            }

            try
            {
                using (var readStream = file.OpenReadStream())
                {
                    using (var img = Image.FromStream(readStream))
                    {

                        if (!img.RawFormat.Equals(ImageFormat.Jpeg) && !img.RawFormat.Equals(ImageFormat.Png))
                        {
                            throw new Exception("Uploaded file is not recognized as an image.");
                        }

                        var fileName = key + ".png";

                        try
                        {
                            // Check if previous avatar exists
                            var avatar = _avatarDirectoryInfo.GetFile(fileName);
                            if (avatar.Exists)
                            {
                                // delete the file
                                avatar.Delete();
                            }

                            // finally, build the image
                            AvatarBuilder.Build(readStream, fileName);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Uploaded file is not recognized as a valid image.", ex);
                        }

                        return key;
                    }
                }
            }
            catch(Exception)
            {
                throw new Exception("Uploaded file is not recognized as an image.");
            }
        }

        public Stream GetAvatarStream(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var avatar = _avatarDirectoryInfo.GetFile(key + ".jpg");
            if (avatar.Exists)
            {
                return avatar.Open(FileMode.Open);
            }

            return null;
        }
    }
}
