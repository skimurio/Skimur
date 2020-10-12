using System;
using ServiceStack.OrmLite;
using Skimur.Backend.Sql;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.Services.Impl
{
    public class SubCssService : ISubCssService
    {
        private readonly IDbConnectionProvider _conn;

        public SubCssService(IDbConnectionProvider conn)
        {
            _conn = conn;
        }

        public SubCss GetStylesForSub(Guid subId)
        {
            return _conn.Perform(conn =>
            {
                return conn.Single<SubCss>(x => x.SubId == subId);
            });
        }

        public void UpdateStylesForSub(SubCss styles)
        {
            _conn.Perform(conn =>
            {
                var existing = conn.Single<SubCss>(x => x.SubId == styles.SubId);

                if (existing != null) {

                    existing.CssType = styles.CssType;
                    existing.Embedded = styles.Embedded;
                    existing.ExternalCss = styles.ExternalCss;
                    existing.GitHubCssProjectName = styles.GitHubCssProjectName;
                    existing.GitHubCssProjectTag = styles.GitHubCssProjectTag;
                    existing.GitHubLessProjectName = styles.GitHubLessProjectName;
                    existing.GitHubLessProjectTag = styles.GitHubLessProjectTag;
                    conn.Update(existing);

                } else {

                    existing = new SubCss();
                    existing.Id = Guid.NewGuid();
                    existing.SubId = styles.SubId;
                    existing.CssType = styles.CssType;
                    existing.Embedded = styles.Embedded;
                    existing.ExternalCss = styles.ExternalCss;
                    existing.GitHubCssProjectName = styles.GitHubCssProjectName;
                    existing.GitHubCssProjectTag = styles.GitHubCssProjectTag;
                    existing.GitHubLessProjectName = styles.GitHubLessProjectName;
                    existing.GitHubLessProjectTag = styles.GitHubLessProjectTag;
                    conn.Insert(existing);

                    styles.Id = existing.Id;

                }
            });
        }
    }
}
