﻿using System;
using System.Collections.Generic;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.ReadModel
{
    public interface IPostDao
    {
        Post GetPostById(Guid id);

        SeekedList<Guid> GetPosts(List<Guid> subs = null,
            PostsSortBy sortby = PostsSortBy.New,
            PostsTimeFilter timeFilter = PostsTimeFilter.All,
            Guid? userId = null,
            bool hideRemovedPosts = true,
            bool showDeleted = false,
            bool onlyAll = false,
            bool? nsfw = null,
            bool? sticky = null,
            bool stickyFirst = false,
            int? skip = null,
            int? take = null);

        SeekedList<Guid> QueryPosts(string text,
            Guid? subId = null,
            PostsSearchSortBy sortBy = PostsSearchSortBy.Relevance,
            PostsTimeFilter timeFilter = PostsTimeFilter.All,
            Guid? userId = null,
            bool hideRemovedPosts = true,
            bool showDeleted = false,
            bool? nsfw = null,
            int? skip = null,
            int? take = null);

        SeekedList<Guid> GetUnmoderatedPosts(List<Guid> subs = null, int? skip = null, int? take = null);

        SeekedList<Guid> GetReportedPosts(List<Guid> subs = null, int? skip = null, int? take = null);
    }

    public enum PostsSearchSortBy
    {
        Relevance,
        Top,
        New,
        Comments
    }

    public enum PostsSortBy
    {
        Hot,
        New,
        Rising,
        Controversial,
        Top
    }

    public enum PostsTimeFilter
    {
        All,
        Hour,
        Day,
        Week,
        Month,
        Year
    }
}
