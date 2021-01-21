using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface IModeratorWrapper
    {
        List<ModeratorWrapped> Wrap(List<Moderator> moderators, User currentUser = null);

        ModeratorWrapped Wrap(Moderator moderator, User currentUser = null);
    }
}
