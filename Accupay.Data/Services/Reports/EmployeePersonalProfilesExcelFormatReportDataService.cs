using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AccuPay.Data.Services.Reports
{
    public class EmployeePersonalProfilesExcelFormatReportDataService : StoredProcedureDataService
    {
        public EmployeePersonalProfilesExcelFormatReportDataService(PayrollContext context) : base(context)
        {
        }

        public DataTable GetData(int organizationId)
        {
            var procedureCall = string.Format("CALL PRINT_employee_profiles({0});", organizationId);

            var data = CallRawSql(procedureCall);

            return data;
        }
    }
}