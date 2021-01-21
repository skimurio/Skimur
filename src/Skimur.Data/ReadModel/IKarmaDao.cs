using System;
using System.Collections.Generic;
using Skimur.Data.Services;

namespace Skimur.Data.ReadModel
{
    public interface IKarmaDao
    {
        Dictionary<KarmaReportKey, int> GetKarma(Guid userId);
    }
}
