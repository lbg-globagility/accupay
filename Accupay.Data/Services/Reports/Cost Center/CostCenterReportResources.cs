using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class CostCenterReportResources
    {
        private readonly PayrollContext _context;
        private readonly ListOfValueService _listOfValueService;
        private readonly AllowanceRepository _allowanceRepository;
        private readonly CalendarService _calendarService;
        private DateTime _selectedMonth;

        public List<TimeEntry> TimeEntries { get; private set; }
        public List<ActualTimeEntry> ActualTimeEntries { get; private set; }
        public List<TimePeriod> ReportTimePeriods { get; private set; }
        public TimePeriod ReportPeriod { get; private set; }
        public List<Salary> Salaries { get; private set; }
        public List<Paystub> Paystubs { get; private set; }
        public List<SocialSecurityBracket> SocialSecurityBrackets { get; private set; }
        public List<LoanTransaction> HmoLoans { get; private set; }
        public List<PayPeriod> PayPeriods { get; private set; }
        public ListOfValueCollection Settings { get; private set; }
        public CalendarCollection CalendarCollection { get; private set; }
        public List<Allowance> DailyAllowances { get; private set; }
        public List<Employee> Employees { get; private set; }

        public CostCenterReportResources(
            PayrollContext context,
            ListOfValueService listOfValueService,
            AllowanceRepository allowanceRepository,
            CalendarService calendarService)
        {
            _context = context;
            _listOfValueService = listOfValueService;
            _allowanceRepository = allowanceRepository;
            _calendarService = calendarService;
        }

        public async Task Load(DateTime selectedMonth)
        {
            _selectedMonth = selectedMonth;

            ReportTimePeriods = GetPayPeriod();

            DateTime startDate = new DateTime[] { ReportTimePeriods[0].Start, ReportTimePeriods[1].Start }.Min();
            DateTime endDate = new DateTime[] { ReportTimePeriods[0].End, ReportTimePeriods[1].End }.Max();

            ReportPeriod = new TimePeriod(startDate, endDate);

            var dateForCheckingLastWorkingDay = PayrollTools
                .GetPreviousCutoffDateForCheckingLastWorkingDay(ReportPeriod.Start);

            TimeEntries = new List<TimeEntry>();

            TimeEntries = await _context.TimeEntries
                .Include(t => t.Employee)
                .Where(t => t.Date >= dateForCheckingLastWorkingDay)
                .Where(t => t.Date <= ReportPeriod.End)
                .ToListAsync();

            ActualTimeEntries = await _context.ActualTimeEntries
                .Where(t => t.Date >= dateForCheckingLastWorkingDay)
                .Where(t => t.Date <= ReportPeriod.End)
                .ToListAsync();

            Salaries = await _context.Salaries
                .Where(s => s.EffectiveFrom <= ReportPeriod.End)
                .ToListAsync();

            Paystubs = await _context.Paystubs
                .Include(p => p.ThirteenthMonthPay)
                .Include(p => p.PayPeriod)
                .Where(p => p.PayPeriod.PayFromDate >= ReportPeriod.Start)
                .Where(p => p.PayPeriod.PayToDate <= ReportPeriod.End)
                .ToListAsync();

            var taxEffectivityDate = new DateTime(_selectedMonth.Year, _selectedMonth.Month, 1);

            SocialSecurityBrackets = await _context.SocialSecurityBrackets
                .Where(s => taxEffectivityDate >= s.EffectiveDateFrom)
                .Where(s => taxEffectivityDate <= s.EffectiveDateTo)
                .ToListAsync();

            HmoLoans = await _context.LoanTransactions
                .Include(l => l.Paystub)
                .Include(p => p.PayPeriod)
                .Include(p => p.LoanSchedule.LoanType)
                .Where(p => p.PayPeriod.PayFromDate >= ReportPeriod.Start)
                .Where(p => p.PayPeriod.PayToDate <= ReportPeriod.End)
                .Where(p => p.LoanSchedule.LoanType.Name == ProductConstant.HMO_LOAN)
                .ToListAsync();

            Employees = await _context.Employees
                .Where(x => x.IsDaily)
                .ToListAsync();

            var organizationIds = Employees
                .GroupBy(x => x.OrganizationID.Value)
                .Select(x => x.Key)
                .ToArray();

            PayPeriods = await _context.PayPeriods
                .Where(x => organizationIds.Contains(x.OrganizationID.Value))
                .Where(x => x.Year == _selectedMonth.Year)
                .Where(x => x.Month == _selectedMonth.Month)
                .ToListAsync();

            Settings = await _listOfValueService.CreateAsync();

            CalendarCollection = _calendarService.GetCalendarCollection(ReportPeriod);

            DailyAllowances = GetDailyAllowances(organizationIds);
        }

        private List<Allowance> GetDailyAllowances(int[] organizationIds)
        {
            var dailyAllowances = new List<Allowance>();
            foreach (var organizationId in organizationIds)
            {
                var allowances = _allowanceRepository.GetByPayPeriodWithProduct(
                        organizationId: organizationId,
                        timePeriod: ReportPeriod)
                    .ToList();

                if (allowances.Where(x => x.IsDaily).Any())
                {
                    dailyAllowances.AddRange(allowances.Where(x => x.IsDaily).ToList());
                }
            }

            return dailyAllowances;
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