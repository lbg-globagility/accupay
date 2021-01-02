using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Reports.Employees_Personal_Information;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.Reports
{
    public class EmployeePersonalProfilesExcelFormatReportDataService : StoredProcedureDataService
    {
        private readonly IEmployeeQueryBuilder _employeeQueryBuilder;

        public EmployeePersonalProfilesExcelFormatReportDataService(
            PayrollContext context,
            IEmployeeQueryBuilder employeeQueryBuilder) : base(context)
        {
            _employeeQueryBuilder = employeeQueryBuilder;
        }

        public async Task<ICollection<EmployeeRow>> GetData(int organizationId)
        {
            var employees = await _employeeQueryBuilder
                .IncludePosition()
                .IncludeBranch()
                .IncludeAgency()
                .ToListAsync(organizationId);

            return employees
               .OrderBy(x => x.FullNameWithMiddleInitialLastNameFirst)
               .Select(x => new EmployeeRow(x)).ToList();
        }
    }
}
