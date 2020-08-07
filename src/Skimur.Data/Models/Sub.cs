using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Subs")]
    public class Sub
    {
        /// <summary>
        /// The id of the given sub
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// When this sub was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The name of the sub
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the sub
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// the sidebar text for the sub
        /// </summary>
        public string SidebarText { get; set; }

        /// <summary>
        /// The formatted sidebar text
        /// </summary>
        public string SidebarTextFormatted { get; set; }

        /// <summary>
        /// The submission button text
        /// </summary>
        public string SubmissionText { get; set; }

        /// <summary>
        /// Formatted submission button text
        /// </summary>
        public string SubmissionTextFormatted { get; set; }

        /// <summary>
        /// is this sub a default sub for users?
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// The number of subscribers for this sub
        /// </summary>
        public int Subscribers { get; set; }

        /// <summary>
        /// The type of sub this is
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Who this sub was created by
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Determines if this is an nsfw sub
        /// </summary>
        [Alias("IsNsfw")]
        public bool Nsfw { get; set; }

        /// <summary>
        /// determins if this sub is in /s/all
        /// nsfw subs are not in /s/all by default
        /// </summary>
        public bool InAll { get; set; }

        [Ignore]
        public SubType SubType
        {
            get
            {
                return (SubType)Type;
            }

            set
            {
                Type = (int)value;
            }
        }

    }

    public enum SubType
    {
        Public,
        Restricted,
        Private
    }
}
