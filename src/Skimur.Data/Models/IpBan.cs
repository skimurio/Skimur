using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("IpBans")]
    public class IpBan
    {
        /// <summary>
        /// ban id
        /// </summary>
        public Guid Id;

        public string IpAddress;

        public Guid BannedBy;

        public string Reason;

        public bool IsActive;

        public DateTime? Expires;

        public DateTime CreatedAt;
    }
}
