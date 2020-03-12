using Skimur.Settings;

namespace Skimur.Common.Email
{
    public class EmailServerSettings : ISettings
    {
        public EmailServerSettings()
        {
            FromEmail = "no-reply@skimur.io";
            FromName = "Skimur";
            Host = "localhost";
            Port = 25;
        }

        /// <summary>
        /// The from name
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The from email
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// The host of the server
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port of the server
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The username for the server
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password for the server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Should we use SSL?
        /// </summary>
        public bool EnableSSL { get; set; }

        /// <summary>
        /// Use default credentials?
        /// </summary>
        public bool UseDefaultCredentials { get; set; }
    }
}
