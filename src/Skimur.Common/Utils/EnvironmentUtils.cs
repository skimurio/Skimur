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

        public static bool IsContainer
        {
            get
            {
                return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            }
        }
    }
}
