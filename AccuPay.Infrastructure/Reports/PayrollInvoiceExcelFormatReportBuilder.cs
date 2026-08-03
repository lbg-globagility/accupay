using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Reports;
using System;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports
{
    public class PayrollInvoiceExcelFormatReportBuilder : IPayrollInvoiceExcelFormatReportBuilder
    {
        public PayrollInvoiceExcelFormatReportBuilder()
        {

        }

        public Task<string> GenerateReportAsync(int organizationId,
            bool actualSwitch,
            PayPeriod startPeriod,
            PayPeriod endPeriod,
            string saveFileDiretory)
        {
            throw new NotImplementedException();
        }
    }
}
