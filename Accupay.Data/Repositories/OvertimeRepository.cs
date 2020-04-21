using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OvertimeRepository
    {
        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                Overtime.StatusPending,
                Overtime.StatusApproved
            };
        }

        public async Task<Overtime> GetByIdAsync(int? id)
        {
            using (var context = new PayrollContext())
            {
                return await context.Overtimes.FirstOrDefaultAsync(l => l.RowID.Value == id.Value);
            }
        }

        public async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int? employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Overtimes.Where(l => l.EmployeeID.Value == employeeId.Value).ToListAsync();
            }
        }

        public async Task DeleteAsync(int? id)
        {
            using (var context = new PayrollContext())
            {
                var overtime = await GetByIdAsync(id);

                context.Remove(overtime);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteManyAsync(IList<int?> ids)
        {
            using (var context = new PayrollContext())
            {
                var overtimes = await context.Overtimes.
                    Where(o => ids.Contains(o.RowID)).
                    ToListAsync();

                context.RemoveRange(overtimes);

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveManyAsync(int organizationID, int userID, List<Overtime> currentOvertimes)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var overtime in currentOvertimes)
                {
                    await this.InternalSaveAsync(organizationID, userID, overtime, context);

                    await context.SaveChangesAsync();
                }
            }
        }

        internal async Task InternalSaveAsync(int organizationID, int userID, Overtime overtime, PayrollContext passedContext = null/* TODO Change to default(_) if this is not a reference type */)
        {
            overtime.OrganizationID = organizationID;
            if (overtime.OTStartTime.HasValue)
                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();
            if (overtime.OTEndTime.HasValue)
                overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();

            if (passedContext == null)
            {
                var newContext = new PayrollContext();

                using (newContext)
                {
                    await SaveAsyncFunction(userID, overtime, newContext);
                }
            }
            else
                await SaveAsyncFunction(userID, overtime, passedContext);
        }

        public async Task SaveAsync(int organizationID, int userID, Overtime overtime)
        {
            await this.InternalSaveAsync(organizationID, userID, overtime);
        }

        private async Task SaveAsyncFunction(int userID, Overtime overtime, PayrollContext context)
        {
            if (overtime.RowID == null)
            {
                overtime.CreatedBy = userID;
                context.Overtimes.Add(overtime);
            }
            else
                await UpdateAsync(userID, overtime, context);

            await context.SaveChangesAsync();
        }

        private async Task UpdateAsync(int userID, Overtime overtime, PayrollContext context)
        {
            var currentOfficialBusiness = await context.Overtimes.FirstOrDefaultAsync(l => l.RowID == overtime.RowID);

            if (currentOfficialBusiness == null)
                return;

            currentOfficialBusiness.LastUpdBy = userID;
            currentOfficialBusiness.OTStartTime = overtime.OTStartTime;
            currentOfficialBusiness.OTEndTime = overtime.OTEndTime;
            currentOfficialBusiness.OTStartDate = overtime.OTStartDate;
            currentOfficialBusiness.OTEndDate = overtime.OTEndDate;
            currentOfficialBusiness.Reason = overtime.Reason;
            currentOfficialBusiness.Comments = overtime.Comments;
            currentOfficialBusiness.Status = overtime.Status;
        }
    }
}