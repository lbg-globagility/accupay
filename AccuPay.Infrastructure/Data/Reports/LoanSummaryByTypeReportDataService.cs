using AccuPay.Core.Interfaces;
using System;
using System.Data;

namespace AccuPay.Infrastructure.Data
{
    public class LoanSummaryByTypeReportDataService : StoredProcedureDataService, ILoanSummaryByTypeReportDataService
    {
        public LoanSummaryByTypeReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new object[,]
            {
                {"OrganizID", organizationId},
                {"PayDateFrom", dateFrom},
                {"PayDateTo", dateTo}
            };

            var data = CallProcedure("RPT_LoansByType", parameters);

            return data;
        }
    }
}
