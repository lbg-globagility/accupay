using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Services.Reports.Cost_Center
{
    internal class CostCenterReportResources
    {
        private readonly PayrollContext _context;
        private readonly DateTime _selectedMonth;
        private DateTime dateForCheckingLastWorkingDay;

        public List<TimeEntry> TimeEntries { get; private set; }

        public CostCenterReportResources(
            PayrollContext context,
            DateTime selectedMonth)
        {
            _context = context;
            _selectedMonth = selectedMonth;
        }

        public async Task Load()
        {
            var payPeriods = GetPayPeriod();
            DateTime startDate = new DateTime[] { payPeriods[0].Start, payPeriods[1].Start }.Min();
            DateTime endDate = new DateTime[] { payPeriods[0].End, payPeriods[1].End }.Max();

            var reportPeriod = new TimePeriod(startDate, endDate);

            var dateForCheckingLastWorkingDay = PayrollTools
                .GetPreviousCutoffDateForCheckingLastWorkingDay(reportPeriod.Start);

            TimeEntries = await _context.TimeEntries
                .Include(t => t.Employee)
                .Where(t => t.Date >= dateForCheckingLastWorkingDay)
                .Where(t => t.Date <= reportPeriod.End)
                .ToListAsync();
        }

        private List<TimePeriod> GetPayPeriod()
        {
            // get a random organizationId just to get a random payperiodId
            var organizationId = _context.Organizations.Select(x => x.RowID).FirstOrDefault();
            if (organizationId == null) return null;

            // get a random payperiod just to get the first day and last day of the payroll month
            var payPeriods = _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.IsSemiMonthly)
                .Where(p => p.Year == _selectedMonth.Year)
                .Where(p => p.Month == _selectedMonth.Month)
                .ToList();

            if (payPeriods.Count != 2)
                throw new Exception($"Pay periods on the selected month was {payPeriods.Count} instead of 2 (First half, End of the month)");

            return new List<TimePeriod>()
            {
                new TimePeriod(payPeriods[0].PayFromDate, payPeriods[0].PayToDate),
                new TimePeriod(payPeriods[1].PayFromDate, payPeriods[1].PayToDate)
            };
        }
    }
}