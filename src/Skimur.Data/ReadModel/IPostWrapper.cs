﻿using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface IPostWrapper
    {
        List<PostWrapped> Wrap(List<Guid> postIds, User currentUser = null);

        PostWrapped Wrap(Guid postId, User currentUser = null);
    }
}
