using AccuPay.Core.Entities.LeaveReset;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class LeaveResetRepository : ILeaveResetRepository
    {
        private const string LEAVE_RESET_POLICY = "LeaveResetPolicy";
        private readonly PayrollContext _payrollContext;

        public LeaveResetRepository(PayrollContext payrollContext)
        {
            _payrollContext = payrollContext;
        }

        public async Task<LeaveReset> GetByOrganizationIdAndDate(int organizationId, TimePeriod timePeriod)
        {
            var leaveResets = await _payrollContext.LeaveResets.
                    Include(l => l.LeaveTypeRenewables).
                        ThenInclude(l => l.Product).
                    Include(l => l.LeaveTenures).
                Where(l => l.OrganizationId == organizationId).
                Where(l => l.EffectiveDate <= timePeriod.Start).
                Where(l => l.EffectiveDate <= timePeriod.End).
                OrderByDescending(l => l.EffectiveDate).
                ToListAsync();

            return leaveResets.FirstOrDefault();
        }

        public async Task<ILeaveResetPolicy> GetLeaveResetPolicyAsync()
        {
            var leaveResets = await _payrollContext.ListOfValues.
                AsNoTracking().
                Where(l => l.Type == LEAVE_RESET_POLICY).
                ToListAsync();
            return new LeaveResetPolicy(
                new ListOfValueCollection(leaveResets));
        }
    }
}
