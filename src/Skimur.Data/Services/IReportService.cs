using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.Services
{
    public interface IReportService
    {
        List<Report.CommentReport> GetReportsForComment(Guid commentId);

        List<Report.PostReport> GetReportsForPost(Guid postId);

        void ReportComment(Guid commentId, Guid reportedBy, string reason);

        void ReportPost(Guid postId, Guid reportedBy, string reason);

        void RemoveReportsForPost(Guid postId);

        void RemoveReportsForComment(Guid commentId);
    }
}
