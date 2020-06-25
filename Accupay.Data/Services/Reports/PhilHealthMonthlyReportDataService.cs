using System;
using System.Data;

namespace AccuPay.Data.Services
{
    public class PhilHealthMonthlyReportDataService : StoredProcedureDataService
    {
        public PhilHealthMonthlyReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime date)
        {
            object[,] @params = new object[3, 3];
            @params[0, 0] = "OrganizID";
            @params[1, 0] = "paramDate";
            @params[0, 1] = organizationId;
            @params[1, 1] = date.ToString("yyyy-MM-dd");

            var data = (DataTable)CallProcedure("RPT_PhilHealth_Monthly", @params);

            return data;
        }
    }
}
