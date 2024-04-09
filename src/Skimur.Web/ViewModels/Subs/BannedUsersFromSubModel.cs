using Skimur.Data.Models;
using Skimur.Data.ReadModel;

namespace Skimur.Web.ViewModels.Subs
{
    public class BannedUsersFromSubModel
    {
        public Sub Sub { get; set; }

        public PagedList<SubUserBanWrapped> Users { get; set; }

        public string Query { get; set; }


        public BanUserModel BanUser { get; set; }
    }
}
