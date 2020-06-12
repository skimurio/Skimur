using System.IO;
using Microsoft.AspNetCore.Http;

namespace Skimur.Web.Services
{
    public interface IAvatarService
    {
        string UploadAvatar(IFormFile file, string key);

        Stream GetAvatarStream(string key);
    }
}
