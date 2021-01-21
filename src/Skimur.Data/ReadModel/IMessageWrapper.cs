using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface IMessageWrapper
    {
        List<MessageWrapped> Wrap(List<Guid> messageIds, User currentUser);

        MessageWrapped Wrap(Guid messageId, User currentUser);
    }
}
