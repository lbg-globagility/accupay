using AccuPay.Core.Repositories;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class ThirteenthMonthSummaryReportDataService : StoredProcedureDataService
    {
        private readonly PaystubRepository _paystubRepository;

        public ThirteenthMonthSummaryReportDataService(PayrollContext context, PaystubRepository paystubRepository) : base(context)
        {
            _paystubRepository = paystubRepository;
        }

        public async Task<DataTable> GetData(int organizationId, TimePeriod timePeriod)
        {
            var datatable = new DataTable();

            datatable.Columns.Add("DatCol1"); // Employee Number
            datatable.Columns.Add("DatCol2"); // Employee Name
            datatable.Columns.Add("DatCol3"); // 13th Month Basic Pay
            datatable.Columns.Add("DatCol4"); // 13th Month Amount

            var paystubs = (await _paystubRepository.GetByTimePeriodWithThirteenthMonthPayAndEmployeeAsync(
                    timePeriod,
                    organizationId))
                .GroupBy(x => x.Employee)
                .OrderBy(x => x.Key?.FullNameWithMiddleInitialLastNameFirst);

            foreach (var paystub in paystubs)
            {
                DataRow newRow = datatable.NewRow();

                newRow["DatCol1"] = paystub.Key?.EmployeeNo;
                newRow["DatCol2"] = paystub.Key?.FullNameLastNameFirst;
                newRow["DatCol3"] = paystub.Sum(x => x.ThirteenthMonthPay?.BasicPay).RoundToString();
                newRow["DatCol4"] = paystub.Sum(x => x.ThirteenthMonthPay?.Amount).RoundToString();

                datatable.Rows.Add(newRow);
            }

            return datatable;
        }
    }
}
