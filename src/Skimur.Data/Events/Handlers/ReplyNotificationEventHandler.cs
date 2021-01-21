﻿using System;
using Skimur.Data.Commands;
using Skimur.Data.Models;
using Skimur.Data.Services;
using Skimur.Messaging;
using Skimur.Messaging.Handling;

namespace Skimur.Data.Events.Handlers
{
    public class ReplyNotificationEventHandler :
        IEventHandler<CommentCreated>,
        IEventHandler<CommentDeleted>
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly ICommandBus _commandBus;
        private readonly IMessageService _messageService;

        public ReplyNotificationEventHandler(ICommentService commentService,
            IPostService postService,
            ICommandBus commandBus,
            IMessageService messageService)
        {
            _commentService = commentService;
            _postService = postService;
            _commandBus = commandBus;
            _messageService = messageService;
        }

        public void Handle(CommentCreated @event)
        {
            var post = _postService.GetPostById(@event.PostId);
            if (post == null)
            {
                return;
            }

            var comment = _commentService.GetCommentById(@event.CommentId);
            if (comment == null)
            {
                return;
            }

            Comment replyToComment = null;

            if (comment.ParentId.HasValue)
            {
                replyToComment = _commentService.GetCommentById(comment.ParentId.Value);
                if (replyToComment == null)
                {
                    return;
                }
            }

            if (replyToComment == null)
            {
                // the user replied to his own post. don't notify
                if (post.UserId == comment.AuthorUserId)
                {
                    return;
                }

                // this is a reply to a post
                if (post.SendReplies)
                {
                    // the user wants to know about replies
                    _commandBus.Send(new SendMessage
                    {
                        AuthorId = comment.AuthorUserId,
                        AuthorIpAddress = comment.AuthorIpAddress,
                        ToUserId = post.UserId,
                        Type = MessageType.PostReply,
                        CommentId = comment.Id
                    });
                }
            }
            else
            {
                // the user replied to his own comment. don't notify
                if (replyToComment.AuthorUserId == comment.AuthorUserId)
                {
                    return;
                }

                // this is a reply to a comment
                if (replyToComment.SendReplies)
                {
                    // the user wants to know about replies
                    _commandBus.Send(new SendMessage
                    {
                        AuthorId = comment.AuthorUserId,
                        AuthorIpAddress = comment.AuthorIpAddress,
                        ToUserId = replyToComment.AuthorUserId,
                        Type = MessageType.CommentReply,
                        CommentId = comment.Id
                    });
                }
            }
        }

        public void Handle(CommentDeleted @event)
        {
            var comment = _commentService.GetCommentById(@event.CommentId);
            if (comment == null || !comment.Deleted)
            {
                return;
            }

            // lets delete any comment notifications that may have been created.
            _messageService.DeleteNotificationsForComment(comment.Id);
        }
    }
}
