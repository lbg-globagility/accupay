using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                var branches = _branchRepository.GetAll();

                var calendarDays = _context.CalendarDays.
                                            Include(t => t.DayType).
                                            Where(t => timePeriod.Start <= t.Date).
                                            Where(t => t.Date <= timePeriod.End).
                                            ToList();

                return new CalendarCollection(payrates, (ICollection<Branch>)branches, calendarDays, organizationId);
            }
            else
                return new CalendarCollection(payrates, organizationId);
        }
    }
}