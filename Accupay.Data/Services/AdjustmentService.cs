using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class AdjustmentService
    {
        public async Task<IEnumerable<IAdjustment>> GetByMultipleEmployeeAndDatePeriodAsync(
                                                                    int organizationId,
                                                                    int[] employeeIds,
                                                                    TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                var adjustmentQuery = GetBaseAdjustmentQuery(
                                    context.Adjustments.Where(a => a.OrganizationID == organizationId),
                                    employeeIds,
                                    timePeriod);

                var actualAdjustmentQuery = GetBaseAdjustmentQuery(
                                    context.ActualAdjustments.Where(a => a.OrganizationID == organizationId),
                                    employeeIds,
                                    timePeriod);

                var adjustments = new List<IAdjustment>();

                adjustments = new List<IAdjustment>(await adjustmentQuery.ToListAsync());
                adjustments.AddRange(new List<IAdjustment>(await actualAdjustmentQuery.ToListAsync()));

                return adjustments;
            }
        }

        private IQueryable<IAdjustment> GetBaseAdjustmentQuery(IQueryable<IAdjustment> query,
                                                                int[] employeeIds,
                                                                TimePeriod timePeriod)
        {
            return query.Include(p => p.Product).
                        Include(p => p.Paystub).
                        Include(p => p.Paystub.PayPeriod).
                        Where(p => p.Paystub.PayPeriod.PayFromDate >= timePeriod.Start).
                        Where(p => p.Paystub.PayPeriod.PayToDate <= timePeriod.End).
                        Where(p => employeeIds.Contains(p.Paystub.EmployeeID.Value));
        }
    }
}