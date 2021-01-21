using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class ReportDao : ReportService, IReportDao
    {
        public ReportDao(IDbConnectionProvider conn) : base(conn)
        {
        }
    }
}
