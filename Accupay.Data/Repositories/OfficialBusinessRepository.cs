using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OfficialBusinessRepository
    {
        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                OfficialBusiness.StatusPending,
                OfficialBusiness.StatusApproved
            };
        }

        public async Task<OfficialBusiness> GetByIdAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                return await context.OfficialBusinesses.
                                FirstOrDefaultAsync(l => l.RowID.Value == id);
            }
        }

        public async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.OfficialBusinesses.
                                    Where(l => l.EmployeeID.Value == employeeId).
                                    ToListAsync();
            }
        }

        public IEnumerable<OfficialBusiness> GetAllApprovedBetweenDates(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return context.OfficialBusinesses.
                                Where(l => l.OrganizationID == organizationId).
                                Where(l => l.Status == OfficialBusiness.StatusApproved).
                                Where(l => timePeriod.Start >= l.StartDate).
                                Where(l => timePeriod.End <= l.EndDate).
                                ToList();
            }
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var officialBusiness = await GetByIdAsync(id);

                context.Remove(officialBusiness);

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveManyAsync(List<OfficialBusiness> currentOfficialBusinesses,
                                        int organizationId,
                                        int userId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var officialBusiness in currentOfficialBusinesses)
                {
                    await this.InternalSaveAsync(officialBusiness,
                                        organizationId: organizationId,
                                        userId: userId,
                                        context: context);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SaveAsync(OfficialBusiness officialBusiness,
                                    int organizationId,
                                    int userId)
        {
            await InternalSaveAsync(officialBusiness,
                                    organizationId: organizationId,
                                    userId: userId);
        }

        internal async Task InternalSaveAsync(OfficialBusiness officialBusiness,
                                    int organizationId,
                                    int userId,
                                    PayrollContext context = null/* TODO Change to default(_) if this is not a reference type */)
        {
            officialBusiness.OrganizationID = organizationId;
            if (officialBusiness.StartTime.HasValue)
                officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            if (officialBusiness.EndTime.HasValue)
                officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            if (context == null)
            {
                using (context = new PayrollContext())
                {
                    await SaveAsyncFunction(officialBusiness, context, userId);

                    await context.SaveChangesAsync();
                }
            }
            else
                await SaveAsyncFunction(officialBusiness, context, userId);
        }

        private async Task SaveAsyncFunction(OfficialBusiness officialBusiness, PayrollContext context, int userId)
        {
            if (context.OfficialBusinesses.
                    Where(l => officialBusiness.RowID == null ? true : officialBusiness.RowID != l.RowID).
                    Where(l => l.EmployeeID.Value == officialBusiness.EmployeeID.Value).
                    Where(l => (l.StartDate.HasValue && officialBusiness.StartDate.HasValue && l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date)).
                    Any())
                throw new ArgumentException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            if (officialBusiness.RowID == null)
            {
                officialBusiness.CreatedBy = userId;
                context.OfficialBusinesses.Add(officialBusiness);
            }
            else
                await UpdateAsync(officialBusiness, context, userId);
        }

        private async Task UpdateAsync(OfficialBusiness officialBusiness, PayrollContext context, int userId)
        {
            var currentOfficialBusiness = await context.OfficialBusinesses.
                                                FirstOrDefaultAsync(l => l.RowID == officialBusiness.RowID);

            if (currentOfficialBusiness == null)
                return;

            currentOfficialBusiness.LastUpdBy = userId;
            currentOfficialBusiness.StartTime = officialBusiness.StartTime;
            currentOfficialBusiness.EndTime = officialBusiness.EndTime;
            currentOfficialBusiness.StartDate = officialBusiness.StartDate;
            currentOfficialBusiness.EndDate = officialBusiness.EndDate;
            currentOfficialBusiness.Reason = officialBusiness.Reason;
            currentOfficialBusiness.Comments = officialBusiness.Comments;
            currentOfficialBusiness.Status = officialBusiness.Status;
        }

        #endregion CRUD
    }
}