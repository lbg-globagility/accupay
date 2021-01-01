using System;
using System.Data;

namespace AccuPay.Core.Services
{
    public class PhilHealthMonthlyReportDataService : StoredProcedureDataService
    {
        public PhilHealthMonthlyReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime date)
        {
            object[,] parameters = new object[3, 3];
            parameters[0, 0] = "OrganizID";
            parameters[1, 0] = "paramDate";
            parameters[0, 1] = organizationId;
            parameters[1, 1] = date.ToString("yyyy-MM-dd");

            var data = (DataTable)CallProcedure("RPT_PhilHealth_Monthly", parameters);

            return data;
        }
    }
}
