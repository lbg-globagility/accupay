using AccuPay.Core.Helpers;
using AccuPay.Utilities.Extensions;
using System.Data;
using System.Linq;

namespace AccuPay.Core.Services
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
            var distributionTypes = new string[] {
                PayrollSummaryCategory.Cash.ToTrimmedLowerCase(),
                PayrollSummaryCategory.DirectDeposit.ToTrimmedLowerCase(),
                PayrollSummaryCategory.All.ToTrimmedLowerCase() };

            if (distributionTypes.Contains(salaryDistributionType.ToTrimmedLowerCase()) == false)
            {
                // this means that this will show all, no filters
                salaryDistributionType = PayrollSummaryCategory.All;
            }

            var procedureCall = string.Format("CALL PAYROLLSUMMARY2({0}, {1}, {2}, {3}, {4}, {5});",
                organizationId,
                payPeriodFromId,
                payPeriodToId,
                isActual,
                salaryDistributionType == PayrollSummaryCategory.All ? "null" : $"'{salaryDistributionType}'",
                keepInOneSheet);

            var data = CallRawSql(procedureCall);

            return data;
        }
    }
}