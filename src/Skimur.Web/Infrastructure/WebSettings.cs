using System;
using System.IO;
using Skimur.Settings;

namespace Skimur.Web.Infrastructure
{
    public class WebSettings : ISettings
    {
        public string Announcement { get; set; }

        public string DataDirectory { get; set; }

        public string ThumbnailCache { get; set; }

        public bool ForceHttps { get; set; }

        public string ForceDomain { get; set; }

        public WebSettings()
        {
            DataDirectory = Path.Combine("~","Data");
            ThumbnailCache = Path.Combine("~", "ThumbnailCache");
            ForceHttps = false;
            ForceDomain = null;
        }
    }
}
