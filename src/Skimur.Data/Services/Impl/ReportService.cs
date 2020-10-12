using System;
using System.Collections.Generic;
using ServiceStack.OrmLite;
using Skimur.Data.Models;
using Skimur.Common.Utils;
using Skimur.Backend.Sql;

namespace Skimur.Data.Services.Impl
{
    public class ReportService : IReportService
    {
        private readonly IDbConnectionProvider _conn;

        public ReportService(IDbConnectionProvider connectionProvider)
        {
            _conn = connectionProvider;
        }

        public List<Report.CommentReport> GetReportsForComment(Guid commentId)
        {
            return _conn.Perform(conn => conn.Select(conn.From<Report.CommentReport>().Where(x => x.CommentId == commentId)));
        }

        public List<Report.PostReport> GetReportsForPost(Guid postId)
        {
            return _conn.Perform(conn => conn.Select(conn.From<Report.PostReport>().Where(x => x.PostId == postId)));
        }

        public void ReportComment(Guid commentId, Guid reportedBy, string reason)
        {
            _conn.Perform(conn => conn.Insert(new Report.CommentReport
            {
                Id = Guid.NewGuid(),
                CreatedAt = TimeHelper.CurrentTime(),
                ReportedBy = reportedBy,
                Reason = reason,
                CommentId = commentId
            }));
        }

        public void ReportPost(Guid postId, Guid reportedBy, string reason)
        {
            _conn.Perform(conn => conn.Insert(new Report.PostReport
            {
                Id = Guid.NewGuid(),
                CreatedAt = TimeHelper.CurrentTime(),
                ReportedBy = reportedBy,
                Reason = reason,
                PostId = postId
            }));
        }

        public void RemoveReportsForPost(Guid postId)
        {
            _conn.Perform(conn => conn.Delete<Report.PostReport>(x => x.PostId == postId));
        }

        public void RemoveReportsForComment(Guid commentId)
        {
            _conn.Perform(conn => conn.Delete<Report.CommentReport>(x => x.CommentId == commentId));
        }
    }
}
