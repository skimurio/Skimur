using Skimur.Data.Models;

namespace Skimur.Web.ViewModels
{
    public class AccountHeaderViewModel
    {
        public User CurrentUser { get; set; }

        public int UnreadMessages { get; set; }
    }
}
