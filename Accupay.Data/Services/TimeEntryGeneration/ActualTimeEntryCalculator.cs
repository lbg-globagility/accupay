using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class ActualTimeEntryCalculator
    {
        private readonly Salary _salary;
        private readonly ICollection<ActualTimeEntry> _actualTimeEntries;
        private readonly PolicyHelper _policy;

        public ActualTimeEntryCalculator(Salary salary, ICollection<ActualTimeEntry> actualTimeEntries, PolicyHelper policy)
        {
            _salary = salary;
            _actualTimeEntries = actualTimeEntries;
            _policy = policy;
        }

        public ICollection<ActualTimeEntry> Compute(ICollection<TimeEntry> timeEntries)
        {
            // Changes here should also reflect in BenchmarkPayrollForm.vb
            var actualTimeEntries = new List<ActualTimeEntry>();

            var allowanceRate = (_salary?.BasicSalary ?? 0M) == 0M ?
                                0M :
                                ((_salary?.AllowanceSalary ?? 0) / (_salary?.BasicSalary ?? 0));

            foreach (var timeEntry in timeEntries)
            {
                var actualTimeEntry = _actualTimeEntries.FirstOrDefault(t => t.Date == timeEntry.Date);

                if (actualTimeEntry == null)
                    actualTimeEntry = new ActualTimeEntry()
                    {
                        EmployeeID = timeEntry.EmployeeID,
                        OrganizationID = timeEntry.OrganizationID,
                        Date = timeEntry.Date
                    };

                actualTimeEntry.BasicDayPay = timeEntry.BasicDayPay + (timeEntry.BasicDayPay * allowanceRate);

                actualTimeEntry.RegularPay = timeEntry.RegularPay + (timeEntry.RegularPay * allowanceRate);

                actualTimeEntry.LateDeduction = timeEntry.LateDeduction + (timeEntry.LateDeduction * allowanceRate);
                actualTimeEntry.UndertimeDeduction = timeEntry.UndertimeDeduction + (timeEntry.UndertimeDeduction * allowanceRate);
                actualTimeEntry.AbsentDeduction = timeEntry.AbsentDeduction + (timeEntry.AbsentDeduction * allowanceRate);
                actualTimeEntry.LeavePay = timeEntry.LeavePay + (timeEntry.LeavePay * allowanceRate);

                actualTimeEntry.OvertimePay = timeEntry.OvertimePay;
                if (_policy.AllowanceForOvertime)
                    actualTimeEntry.OvertimePay += timeEntry.OvertimePay * allowanceRate;

                actualTimeEntry.NightDiffPay = timeEntry.NightDiffPay;
                if (_policy.AllowanceForNightDiff)
                    actualTimeEntry.NightDiffPay += timeEntry.NightDiffPay * allowanceRate;

                actualTimeEntry.NightDiffOTPay = timeEntry.NightDiffOTPay;
                if (_policy.AllowanceForNightDiffOT)
                    actualTimeEntry.NightDiffOTPay += timeEntry.NightDiffOTPay * allowanceRate;

                actualTimeEntry.RestDayPay = timeEntry.RestDayPay;
                if (_policy.AllowanceForRestDay)
                    actualTimeEntry.RestDayPay += timeEntry.RestDayPay * allowanceRate;

                actualTimeEntry.RestDayOTPay = timeEntry.RestDayOTPay;
                if (_policy.AllowanceForRestDayOT)
                    actualTimeEntry.RestDayOTPay += timeEntry.RestDayOTPay * allowanceRate;

                actualTimeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayPay;
                actualTimeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTPay;
                actualTimeEntry.RegularHolidayPay = timeEntry.RegularHolidayPay;
                actualTimeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTPay;
                if (_policy.AllowanceForHoliday)
                {
                    actualTimeEntry.SpecialHolidayPay += timeEntry.SpecialHolidayPay * allowanceRate;
                    actualTimeEntry.SpecialHolidayOTPay += timeEntry.SpecialHolidayOTPay * allowanceRate;
                    actualTimeEntry.RegularHolidayPay += timeEntry.RegularHolidayPay * allowanceRate;
                    actualTimeEntry.RegularHolidayOTPay += timeEntry.RegularHolidayOTPay * allowanceRate;
                }

                actualTimeEntry.TotalDayPay = timeEntry.TotalDayPay + (timeEntry.TotalDayPay * allowanceRate);

                actualTimeEntries.Add(actualTimeEntry);
            }

            return actualTimeEntries;
        }
    }
}
