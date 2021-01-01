using System;
using System.Data;

namespace AccuPay.Core.Services
{
    public class PayslipDataService : StoredProcedureDataService
    {
        public PayslipDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetDefaultData(int organizationId, int payPeriodId, bool isActual)
        {
            var procedureCall = "CALL PrintDefaultPayslip(" + organizationId + "," + payPeriodId + "," + Convert.ToSByte(isActual) + ");";

            return CallRawSql(procedureCall);
        }

        public DataTable GetCinema2000Data(int organizationId, int payPeriodId)
        {
            var procedureCall = "CALL RPT_payslip(" + organizationId + "," + payPeriodId + ", TRUE, NULL);";

            return CallRawSql(procedureCall);
        }

        public DataTable GetGoldWingsData(int organizationId, int payPeriodId, bool isActual)
        {
            var procedureCall = "CALL paystub_payslip(" + organizationId + "," + payPeriodId + "," + Convert.ToSByte(isActual) + ");";

            return CallRawSql(procedureCall);
        }
    }
}