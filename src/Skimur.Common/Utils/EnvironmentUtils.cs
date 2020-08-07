using System;

namespace Skimur.Common.Utils
{
    public class EnvironmentUtils
    {
        public static bool IsHeroku
        {
            get
            {
                return Environment.GetEnvironmentVariable("HEROKU") == "true";
            }
        }
    }
}
