using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface IModeratorInviteWrapper
    {
        List<ModeratorInviteWrapped> Wrap(List<ModeratorInvite> invites, User currentUser = null);

        ModeratorInviteWrapped Wrap(ModeratorInvite invite, User currentUser = null);
    }
}
