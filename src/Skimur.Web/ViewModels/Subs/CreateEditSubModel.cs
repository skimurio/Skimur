using System.ComponentModel.DataAnnotations;
using Skimur.Data.Models;

namespace Skimur.Web.ViewModels.Subs
{
    public class CreateEditSubModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        // TODO: Request validation
        [Display(Name = "Sidebar text")]
        public string SidebarText { get; set; }

        // TODO: Request validation
        [Display(Name = "Submission text")]
        public string SubmissionText { get; set; }

        [Display(Name = "Type")]
        public SubType SubType { get; set; }

        [Display(Name = "Is default")]
        public bool? IsDefault { get; set; }

        [Display(Name = "Show in /s/all?")]
        public bool InAll { get; set; }

        [Display(Name = "18 or older content?")]
        public bool Nsfw { get; set; }

        public bool IsEditing { get; set; }
    }
}
