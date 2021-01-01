using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccuPay.Core.Services
{
    public class PagIBIGMonthlyReportDataService : StoredProcedureDataService
    {
        public PagIBIGMonthlyReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime date)
        {
            object[,] parameters = new object[3, 3];
            parameters[0, 0] = "OrganizID";
            parameters[1, 0] = "paramDate";
            parameters[0, 1] = organizationId;
            parameters[1, 1] = date.ToString("yyyy-MM-dd");

            var data = CallProcedure("RPT_PAGIBIG_Monthly", parameters);

            return data;
        }
    }
}
