using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    /// <summary>
    /// Represents a badge
    /// </summary>
    [Alias("Badges")]
    class Badge
    {
        /// <summary>
        /// Badge Id (Primary Key)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the badge
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the badge
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Location to the badge icon
        /// </summary>
        public string Icon { get; set; }
    }
}
