using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccuPay.Core.Services
{
    public class LoanSummaryByEmployeeReportDataService : StoredProcedureDataService
    {
        public LoanSummaryByEmployeeReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime date_from, DateTime date_to)
        {
            var procedureCall = "CALL RPT_loans(" + organizationId + ",'" + date_from.ToString("yyyy-MM-dd") + "','" + date_to.ToString("yyyy-MM-dd") + "', NULL);";

            var data = CallRawSql(procedureCall);

            return data;
        }
    }
}
