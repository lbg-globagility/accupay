using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AccuPay.Data.Services
{
    public class TaxMonthlyReportDataService : StoredProcedureDataService
    {
        private readonly PayPeriodRepository _repository;
        public TaxMonthlyReportDataService(PayrollContext context, PayPeriodRepository repository) : base(context)
        {
            _repository = repository;
        }

        public DataTable GetData(int organizationId, int month, int year)
        {

            var payPeriods = _repository.GetByMonthYearAndPayPrequency(
                                organizationId,
                                month: month,
                                year: year,
                                payFrequencyId: AccuPay.Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId);

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
