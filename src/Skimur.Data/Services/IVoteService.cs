﻿using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.Services
{
    public interface IVoteService
    {
        void VoteForPost(Guid postId, Guid userId, string ipAddress, VoteType voteType);

        void UnvotePost(Guid postId, Guid userId);

        VoteType? GetVoteForUserOnPost(Guid userId, Guid postId);

        Dictionary<Guid, VoteType> GetVotesOnPostsByUser(Guid userId, List<Guid> posts);

        void GetTotalVotesForPost(Guid postId, out int upVotes, out int downVotes);

        void VoteForComment(Guid commentId, Guid userId, string ipAddress, VoteType voteType);

        void UnvoteComment(Guid commentId, Guid userId);

        VoteType? GetVoteForUserOnComment(Guid userId, Guid commentId);

        Dictionary<Guid, VoteType> GetVotesOnCommentsByUser(Guid userId, Guid commentId);

        void GetTotalVotesForComment(Guid commentId, out int upVotes, out int downVotes);
    }
}
