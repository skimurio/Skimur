using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Commands
{
    public class EditSubStyles : ICommandReturns<EditSubStylesResponse>
    {
        public Guid EditedByUserId { get; set; }

        public string SubName { get; set; }

        public Guid? SubId { get; set; }

        public CssType CssType { get; set; }

        public string Embedded { get; set; }

        public string ExternelCss { get; set; }

        public string GitHubCssProjectName { get; set; }

        public string GitHubCssProjectTag { get; set; }

        public string GitHubLessProjectName { get; set; }

        public string GitHubLessProjectTag { get; set; }
    }

    public class EditSubStylesResponse
    {
        public string Error { get; set; }
    }
}
