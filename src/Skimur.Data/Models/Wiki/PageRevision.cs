using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models.Wiki
{
    /// <summary>
    /// Represents a page revision for a wiki
    /// </summary>
    [Alias("WikiPageRevisions")]
    public class PageRevision
    {

        /// <summary>
        /// The Id for this revision
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The page id for this revision
        /// </summary>
        public Guid PageId { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// The date and time that this revision was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

    }
}
