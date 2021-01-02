using AccuPay.Core.Interfaces;
using System.Data;
using System.Linq;

namespace AccuPay.Infrastructure.Data
{
    public class TaxMonthlyReportDataService : StoredProcedureDataService, ITaxMonthlyReportDataService
    {
        private readonly IPayPeriodRepository _repository;

        public TaxMonthlyReportDataService(PayrollContext context, IPayPeriodRepository repository) : base(context)
        {
            _repository = repository;
        }

        public DataTable GetData(int organizationId, int month, int year)
        {
            var payPeriods = _repository.GetByMonthYearAndPayPrequency(
                                organizationId,
                                month: month,
                                year: year,
                                payFrequencyId: AccuPay.Core.Helpers.PayrollTools.PayFrequencySemiMonthlyId);

            var dateFrom = payPeriods.First().PayFromDate;
            var dateTo = payPeriods.Last().PayToDate;

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
