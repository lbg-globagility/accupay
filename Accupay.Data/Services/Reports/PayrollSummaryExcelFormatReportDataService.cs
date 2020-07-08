using System.Data;

namespace AccuPay.Data.Services
{
    public class PayrollSummaryExcelFormatReportDataService : StoredProcedureDataService
    {
        public PayrollSummaryExcelFormatReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(
            int organizationId,
            int payPeriodFromId,
            int payPeriodToId,
            string salaryDistributionType,
            short isActual,
            bool keepInOneSheet)
        {
            var procedureCall = string.Format("CALL PAYROLLSUMMARY2({0}, {1}, {2}, {3}, {4}, {5});",
                organizationId,
                payPeriodFromId,
                payPeriodToId,
                isActual,
                salaryDistributionType == null ? "null" : $"'{salaryDistributionType}'",
                keepInOneSheet);

            var data = CallRawSql(procedureCall);

            return data;
        }
    }
}