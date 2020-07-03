using System;
using System.Data;

namespace AccuPay.Data.Services
{
    public class SSSMonthlyReportDataService : StoredProcedureDataService
    {
        public SSSMonthlyReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId, DateTime dateMonth)
        {
            object[,] parameters = new object[3, 3];
            parameters[0, 0] = "OrganizID";
            parameters[1, 0] = "paramDate";
            parameters[0, 1] = organizationId;
            parameters[1, 1] = dateMonth.ToString("yyyy-MM-dd");

            var data = CallProcedure("RPT_SSS_Monthly", parameters);

            return data;
        }
    }
}