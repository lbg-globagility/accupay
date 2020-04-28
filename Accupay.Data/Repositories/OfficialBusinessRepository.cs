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

        public async Task SaveManyAsync(List<OfficialBusiness> currentOfficialBusinesses)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var officialBusiness in currentOfficialBusinesses)
                {
                    await this.SaveWithContextAsync(officialBusiness, context);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SaveAsync(OfficialBusiness officialBusiness)
        {
            await SaveWithContextAsync(officialBusiness);
        }

        private async Task SaveWithContextAsync(OfficialBusiness officialBusiness,
                                                PayrollContext context = null)
        {
            if (officialBusiness.EmployeeID == null)
                throw new ArgumentException("Employee does not exists.");

            if (officialBusiness.StartTime.HasValue)
                officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            if (officialBusiness.EndTime.HasValue)
                officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            if (context == null)
            {
                using (context = new PayrollContext())
                {
                    await SaveAsyncFunction(officialBusiness, context);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                await SaveAsyncFunction(officialBusiness, context);
            }
        }

        private async Task SaveAsyncFunction(OfficialBusiness officialBusiness, PayrollContext context)
        {
            if (await context.OfficialBusinesses.
                    Where(l => officialBusiness.RowID == null ? true : officialBusiness.RowID != l.RowID).
                    Where(l => l.EmployeeID == officialBusiness.EmployeeID).
                    Where(l => (l.StartDate.HasValue && officialBusiness.StartDate.HasValue && l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date)).
                    AnyAsync())
                throw new ArgumentException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            if (officialBusiness.RowID == null)
            {
                context.OfficialBusinesses.Add(officialBusiness);
            }
            else
            {
                context.OfficialBusinesses.Attach(officialBusiness);
                context.Entry(officialBusiness).State = EntityState.Modified;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<OfficialBusiness> GetByIdAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                return await context.OfficialBusinesses.
                                FirstOrDefaultAsync(l => l.RowID == id);
            }
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.OfficialBusinesses.
                                    Where(l => l.EmployeeID == employeeId).
                                    ToListAsync();
            }
        }

        public IEnumerable<OfficialBusiness> GetAllApprovedByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return context.OfficialBusinesses.
                                Where(l => l.OrganizationID == organizationId).
                                Where(l => timePeriod.Start >= l.StartDate).
                                Where(l => timePeriod.End <= l.EndDate).
                                Where(l => l.Status == OfficialBusiness.StatusApproved).
                                ToList();
            }
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                OfficialBusiness.StatusPending,
                OfficialBusiness.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries
    }
}