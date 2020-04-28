using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.ValueObjects;
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
        #region CRUD

        public async Task DeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var overtime = await GetByIdAsync(id);

                context.Remove(overtime);

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveManyAsync(List<Overtime> currentOvertimes)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var overtime in currentOvertimes)
                {
                    await SaveWithContextAsync(overtime, context);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SaveAsync(Overtime overtime)
        {
            await SaveWithContextAsync(overtime);
        }

        private async Task SaveWithContextAsync(Overtime overtime,
                                                PayrollContext passedContext = null)
        {
            if (overtime.OTStartTime.HasValue)
                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();
            if (overtime.OTEndTime.HasValue)
                overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();

            if (passedContext == null)
            {
                using (var newContext = new PayrollContext())
                {
                    SaveFunction(overtime, newContext);
                    await newContext.SaveChangesAsync();
                }
            }
            else
            {
                SaveFunction(overtime, passedContext);
            }
        }

        private void SaveFunction(Overtime overtime, PayrollContext context)
        {
            if (overtime.RowID == null)
                context.Overtimes.Add(overtime);
            else
                context.Entry(overtime).State = EntityState.Modified;
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

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Overtime> GetByIdAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                return await context.Overtimes.FirstOrDefaultAsync(l => l.RowID == id);
            }
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Overtimes.
                                    Where(l => l.EmployeeID == employeeId).
                                    ToListAsync();
            }
        }

        public async Task<IEnumerable<Overtime>> GetByDatePeriodAsync(
                                                        int organizationId,
                                                        TimePeriod timePeriod,
                                                        OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByTimePeriod(organizationId,
                                                        timePeriod,
                                                        overtimeStatus,
                                                        context).
                                ToListAsync();
            }
        }

        public IEnumerable<Overtime> GetByDatePeriod(int organizationId,
                                                    TimePeriod timePeriod,
                                                    OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod,
                                                    overtimeStatus,
                                                    context).
                            ToList();
            }
        }

        public IEnumerable<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId,
                                                                TimePeriod timePeriod,
                                                                List<int> employeeIdList,
                                                                OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return CreateBaseQueryByTimePeriod(organizationId,
                                                        timePeriod,
                                                        overtimeStatus,
                                                        context).
                                Where(ot => employeeIdList.Contains(ot.EmployeeID.Value)).
                                ToList();
            }
        }

        #endregion List of entities

        #region Others

        public List<string> GetStatusList()
        {
            return new List<string>()
            {
                Overtime.StatusPending,
                Overtime.StatusApproved
            };
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private static IQueryable<Overtime> CreateBaseQueryByTimePeriod(int organizationId,
                                                                        TimePeriod timePeriod,
                                                                        OvertimeStatus overtimeStatus,
                                                                        PayrollContext context)
        {
            var baseQuery = context.Overtimes.
                                Where(ot => ot.OrganizationID == organizationId).
                                Where(o => timePeriod.Start <= o.OTStartDate).
                                Where(o => o.OTStartDate <= timePeriod.End);

            switch (overtimeStatus)
            {
                case OvertimeStatus.All: break;

                case OvertimeStatus.Approved:
                    baseQuery = baseQuery.Where(o => o.Status.Trim().ToLower() ==
                                                Overtime.StatusApproved.ToTrimmedLowerCase());
                    break;

                case OvertimeStatus.Pending:
                    baseQuery = baseQuery.Where(o => o.Status.Trim().ToLower() ==
                                                Overtime.StatusPending.ToTrimmedLowerCase());
                    break;

                default: break;
            }

            return baseQuery;
        }

        #endregion Private helper methods
    }
}