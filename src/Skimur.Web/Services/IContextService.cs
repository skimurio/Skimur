using System;
using System.Collections.Generic;

namespace Skimur.Web.Services
{
    public interface IContextService
    {
        List<Guid> GetSubscribedSubIds();

        bool IsSubscribedToSub(Guid subId);
    }
}
