using AccuPay.Data.Entities;
using AccuPay.Data.Services;
using AccuPay.Web.Employees.Models;
using System.Collections.Generic;

namespace AccuPay.Web.TimeEntries
{
    public class TimeEntryEmployeeDto : EmployeeDto
    {
        public TotalTimeEntryHours TotalTimeEntry { get; private set; }

        internal static TimeEntryEmployeeDto Convert(Employee employee, IEnumerable<TimeEntry> timeEntries)
        {
            if (employee == null) return null;

            var dto = new TimeEntryEmployeeDto();

            dto.ApplyData(employee);

            dto.TotalTimeEntry = TotalTimeEntryCalculator.CalculateHours(timeEntries);

            return dto;
        }
    }
}
