using System;
using System.Collections.Generic;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.Services
{
    public interface IMessageService
    {

        void InsertMessage(Message message);

        int GetNumberOfUnreadMessagesForUser(Guid userID);

        List<Message> GetMessagesByIds(List<Guid> ids);

        Message GetMessageById(Guid id);

        SeekedList<Guid> GetAllMessagesForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetUnreadMessagesForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetPrivateMessagesForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetCommentRepliesForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetPostRepliesForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetMentionsForUser(Guid userId, int? skip = null, int? take = null);

        SeekedList<Guid> GetSentMessagesForUser(Guid userId, int? skip = null, int? take = null);

        List<Guid> GetMessagesForThread(Guid messageId);

        SeekedList<Guid> GetModeratorMailForSubs(List<Guid> subs, int? skip = null, int? take = null);

        SeekedList<Guid> GetSentModeratorMailForSubs(List<Guid> subs, int? skip = null, int? take = null);

        SeekedList<Guid> GetUnreadModeratorMailForSubs(List<Guid> subs, int? skip = null, int? take = null);

        void MarkMessagesAsRead(List<Guid> messages);

        void MarkMessagesAsUnread(List<Guid> messages);

        void DeleteNotificationsForComment(Guid commentId);

        void InsertMention(Guid userId, Guid authorId, Guid? postId, Guid? commentId);

        void DeleteMention(Guid userId, Guid? postId, Guid? commentId);

    }
}
