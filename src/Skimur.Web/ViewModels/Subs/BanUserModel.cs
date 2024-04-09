using System.ComponentModel.DataAnnotations;

namespace Skimur.Web.ViewModels.Subs
{
    public class BanUserModel
    {
        [Required]
        public string Username { get; set; }

        public string ReasonPrivate { get; set; }

        public string ReasonPublic { get; set;}
    }
}
