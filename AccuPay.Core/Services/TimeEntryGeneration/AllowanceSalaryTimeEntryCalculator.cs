using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class AllowanceSalaryTimeEntryCalculator
    {
        private readonly Salary _salary;
        private readonly ICollection<AllowanceSalaryTimeEntry> _allowanceSalaryTimeEntries;
        private readonly IPolicyHelper _policy;
        private readonly Employee _employee;

        public AllowanceSalaryTimeEntryCalculator(Employee employee, Salary salary, ICollection<AllowanceSalaryTimeEntry> allowanceSalaryTimeEntries, IPolicyHelper policy)
        {
            _salary = salary;
            _allowanceSalaryTimeEntries = allowanceSalaryTimeEntries;
            _policy = policy;
            _employee = employee;
        }

        public ICollection<AllowanceSalaryTimeEntry> Compute(ICollection<TimeEntry> timeEntries)
        {
            // Changes here should also reflect in BenchmarkPayrollForm.vb
            var allowanceSalaryTimeEntries = new List<AllowanceSalaryTimeEntry>();

            var allowanceSalaryDailyRate = _salary?.AllowanceSalary ?? 0;
            var allowanceSalaryHourlyRate = allowanceSalaryDailyRate / PayrollTools.WorkHoursPerDay;

            if (_employee.IsMonthly || _employee.IsFixed)
            {
                allowanceSalaryDailyRate = PayrollTools.GetDailyRate(monthlyRate: _salary?.AllowanceSalary ?? 0, workDaysPerYear: _employee.WorkDaysPerYear);
                allowanceSalaryHourlyRate = allowanceSalaryDailyRate / PayrollTools.WorkHoursPerDay;
            }

            foreach (var timeEntry in timeEntries)
            {
                var _allowanceSalaryTimeEntry = _allowanceSalaryTimeEntries.FirstOrDefault(t => t.Date == timeEntry.Date);

                if (_allowanceSalaryTimeEntry == null)
                    _allowanceSalaryTimeEntry = new AllowanceSalaryTimeEntry()
                    {
                        EmployeeID = timeEntry.EmployeeID,
                        OrganizationID = timeEntry.OrganizationID,
                        Date = timeEntry.Date
                    };

                _allowanceSalaryTimeEntry.BasicDayPay = allowanceSalaryDailyRate;

                _allowanceSalaryTimeEntry.RegularPay = timeEntry.RegularHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.LateDeduction = timeEntry.LateHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.UndertimeDeduction = timeEntry.UndertimeHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.AbsentDeduction = timeEntry.AbsentDeduction > 0 ? allowanceSalaryDailyRate : 0;

                _allowanceSalaryTimeEntry.LeavePay = timeEntry.TotalLeaveHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.OvertimePay = timeEntry.OvertimeHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.NightDiffPay = timeEntry.NightDiffHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.NightDiffOTPay = timeEntry.NightDiffOTHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.RestDayPay = timeEntry.RestDayHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.RestDayOTPay = timeEntry.RestDayOTHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.RegularHolidayPay = timeEntry.RegularHolidayHours * allowanceSalaryHourlyRate;
                _allowanceSalaryTimeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTHours * allowanceSalaryHourlyRate;

                _allowanceSalaryTimeEntry.RecomputeTotalDayPay();

                _allowanceSalaryTimeEntry.IsRestDay = timeEntry.IsRestDay;
                _allowanceSalaryTimeEntry.HasShift = timeEntry.HasShift;

                allowanceSalaryTimeEntries.Add(_allowanceSalaryTimeEntry);
            }

            return allowanceSalaryTimeEntries;
        }
    }
}
