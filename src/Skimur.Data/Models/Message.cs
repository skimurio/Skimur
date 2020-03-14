﻿using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Messages")]
    public class Message
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Type { get; set; }

        public Guid? ParentId { get; set; }

        public Guid? FirstMessage { get; set; }

        public bool Deleted { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorIp { get; set; }

        public bool IsNew { get; set; }

        public Guid? ToUser { get; set; }

        public Guid? ToSub { get; set; }

        public Guid? FromSub { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string BodyFormatted { get; set; }

        public Guid? CommentId { get; set; }

        public Guid? PostId { get; set; }

        [Ignore]
        public MessageType MessageType
        {
            get { return (MessageType)Type; }
            set { Type = (int)value; }
        }
    }

    public enum MessageType
    {
        Private,
        CommentReply,
        PostReply,
        Mention
    }
}
