using System;
using System.Data;

namespace AccuPay.Data.Services
{
    public class ThirteenthMonthSummaryReportDataService : StoredProcedureDataService
    {
        public ThirteenthMonthSummaryReportDataService(PayrollContext context) : base(context)
        {

        }

        public DataTable GetData(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            var parameters = new object[,] {
                {
                    "$organizationID",
                    organizationId
                        },
                {
                    "$dateFrom",
                    dateFrom
                        },
                {
                    "$dateTo",
                    dateTo
                }
            };

            var data = CallProcedure("RPT_13thmonthpay", parameters);

            return data;
        }
    }

}
