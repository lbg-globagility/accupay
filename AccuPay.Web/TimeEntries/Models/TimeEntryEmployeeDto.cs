using AccuPay.Data.Entities;
using AccuPay.Data.Services;
using AccuPay.Web.Employees.Models;

namespace AccuPay.Web.TimeEntries
{
    public class TimeEntryEmployeeDto : EmployeeDto
    {
        public TotalTimeEntryHours TotalTimeEntry { get; private set; }

        public static new TimeEntryEmployeeDto Convert(Employee employee)
        {
            if (employee == null) return null;

            var dto = new TimeEntryEmployeeDto();

            dto.ApplyData(employee);

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
