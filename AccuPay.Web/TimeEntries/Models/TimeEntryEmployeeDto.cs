using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Web.Employees.Models;

namespace AccuPay.Web.TimeEntries
{
    public class TimeEntryEmployeeDto : BaseEmployeeDto
    {
        public int Id { get; set; }
        public TotalTimeEntryHours TotalTimeEntry { get; private set; }

        public static TimeEntryEmployeeDto Convert(Employee employee)
        {
            if (employee == null) return null;

            var dto = new TimeEntryEmployeeDto();

            dto.ApplyData(employee);
            dto.Id = employee.RowID.Value;

            if (employee.TimeEntries == null)
            {
                dto.TotalTimeEntry = null;
            }
            else
            {
                dto.TotalTimeEntry = TotalTimeEntryCalculator.CalculateHours(employee.TimeEntries);
            }

            return dto;
        }
    }
}
