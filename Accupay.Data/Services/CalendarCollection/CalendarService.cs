using AccuPay.Data.Enums;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CalendarService
    {
        private readonly PayrollContext _context;
        private readonly BranchRepository _branchRepository;

        public CalendarService(PayrollContext context, BranchRepository branchRepository)
        {
            _context = context;
            _branchRepository = branchRepository;
        }

        public CalendarCollection GetCalendarCollection(
            TimePeriod timePeriod,
            PayRateCalculationBasis calculationBasis,
            int organizationId)
        {
            var payrates = _context.PayRates.
                Where(p => p.OrganizationID == organizationId).
                Where(p => timePeriod.Start <= p.Date).
                Where(p => p.Date <= timePeriod.End).
                ToList();

            if (calculationBasis == PayRateCalculationBasis.Branch)
            {
                var dayType = _context.DayTypes.FirstOrDefault(t => t.Name == "Regular Day");

                var defaultRates = new DefaultRates()
                {
                    RegularRate = dayType.RegularRate,
                    OvertimeRate = dayType.OvertimeRate,
                    NightDiffRate = dayType.NightDiffRate,
                    NightDiffOTRate = dayType.NightDiffOTRate,
                    RestDayRate = dayType.RestDayRate,
                    RestDayOTRate = dayType.RestDayOTRate,
                    RestDayNDRate = dayType.RestDayNDRate,
                    RestDayNDOTRate = dayType.RestDayNDOTRate,
                };

                var branches = _branchRepository.GetAll();

                var calendarDays = _context.CalendarDays.
                    Include(t => t.DayType).
                    Where(t => timePeriod.Start <= t.Date).
                    Where(t => t.Date <= timePeriod.End).
                    ToList();

                return new CalendarCollection(payrates, branches, calendarDays, organizationId, defaultRates);
            }
            else
            {
                var regularDayListofValue = _context.ListOfValues
                    .FirstOrDefault(t => t.Type == "Pay rate" && t.LIC == "Regular Day");

                var values = regularDayListofValue.DisplayValue
                    .Split(',')
                    .Select(t => decimal.Parse(t) / 100)
                    .ToArray();

                var defaultRates = new DefaultRates()
                {
                    RegularRate = values[0],
                    OvertimeRate = values[1],
                    NightDiffRate = values[2],
                    NightDiffOTRate = values[3],
                    RestDayRate = values[4],
                    RestDayOTRate = values[5],
                    RestDayNDRate = values[6],
                    RestDayNDOTRate = values[7],
                };

                return new CalendarCollection(payrates, organizationId, defaultRates);
            }
        }
    }
}