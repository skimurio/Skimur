using System;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public class SubUserBanWrapped
    {
        public SubUserBanWrapped(SubUserBan userBan)
        {
            Item = userBan;
        }

        public SubUserBan Item { get; private set; }

        public User User { get; set; }

        public User BannedBy { get; set; }
    }
}
