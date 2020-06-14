﻿using System;
using System.Collections.Generic;
using Skimur.Common.Utils;
using Skimur.Data.Models;
using Skimur.Data.Services;

namespace Skimur.Data.ReadModel
{
    public interface ICommentDao
    {
        Comment GetCommentById(Guid id);

        List<Comment> GetCommentsByIds(List<Guid> ids);

        List<Comment> GetAllCommentsForPost(Guid postId, CommentSortBy? sortBy = null);

        Dictionary<Guid, double> GetCommentTreeSorter(Guid postId, CommentSortBy sortBy);

        CommentTree GetCommentTree(Guid postId);

        SeekedList<Guid> GetReportedComments(List<Guid> subs = null, int? skip = null, int? take = null);

        int GetNumberOfCommentsForPost(Guid postId);

        SeekedList<Guid> GetCommentsForUser(Guid userId,
            CommentSortBy? sortBy = null,
            CommentsTimeFilter? timeFilter = null,
            bool showDeleted = false,
            int? skip = null,
            int? take = null);
    }

    public enum CommentSortBy
    {
        Best,
        Hot,
        Top,
        New,
        Controversial,
        Old,
        Qa
    }

    public enum CommentsTimeFilter
    {
        All,
        Hour,
        Day,
        Week,
        Month,
        Year
    }
}
