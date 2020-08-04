using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface ICommentWrapper
    {
        List<CommentWrapped> Wrap(List<Guid> commentIds, User currentUser = null);

        CommentWrapped Wrap(Guid commentId, User currentUser = null);
    }
}
