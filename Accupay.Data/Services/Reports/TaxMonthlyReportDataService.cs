using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccuPay.Data.Services
{
    public class TaxMonthlyReportDataService : StoredProcedureDataService
    {
        public TaxMonthlyReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new object[,] {
                {
                    "OrganizID",
                    organizationId
                },
                {
                    "paramDateFrom",
                    dateFrom.ToString("s")
                },
                {
                    "paramDateTo",
                    dateTo.ToString("s")
                }
            };

            var data = CallProcedure("RPT_Tax_Monthly", parameters);

            return data;
        }
    }
}
