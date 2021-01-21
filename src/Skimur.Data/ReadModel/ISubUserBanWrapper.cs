using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface ISubUserBanWrapper
    {
        List<SubUserBanWrapped> Wrap(List<SubUserBan> items);

        SubUserBanWrapped Wrap(SubUserBan item);
    }
}
