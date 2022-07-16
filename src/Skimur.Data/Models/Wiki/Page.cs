using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models.Wiki
{
    /// <summary>
    /// Represents a wiki page
    /// </summary>
    [Alias("WikiPages")]
    public class Page
    {

        /// <summary>
        /// Page Id (Primary Key)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Id of the Sub this wiki page belongs to.
        /// </summary>
        public Guid SubId { get; set; }

        /// <summary>
        /// The name of the Wiki Page
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Content for the wiki page
        /// </summary>
        public string Content { get; set; }

        /// <summary>
		/// Gets or sets the version number of the content, which starts at 0.
		/// </summary>
		/// <value>
		/// The version number.
		/// </value>
		public int VersionNumber { get; set; }

        /// <summary>
        /// The revision id
        /// </summary>
        public Guid? RevisionId { get; set; }

        /// <summary>
        /// The date and time that this page was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time that this page was updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }


        /// <summary>
        /// The date and time that this page was deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
