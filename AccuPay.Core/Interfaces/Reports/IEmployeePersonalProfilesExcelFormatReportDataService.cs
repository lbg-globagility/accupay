using AccuPay.Core.Services.Reports.Employees_Personal_Information;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeePersonalProfilesExcelFormatReportDataService
    {
        Task<ICollection<EmployeeRow>> GetData(int organizationId);
    }
}
