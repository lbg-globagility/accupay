using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccuPay.Core.Services
{
    public class LoanSummaryByTypeReportDataService : StoredProcedureDataService
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
