using System.Drawing;
using System.IO;

namespace Skimur.Data.Services
{
    public interface IPostThumbnailService
    {
        string UploadImage(Image image);

        Stream GetImage(string thumb);
    }
}
