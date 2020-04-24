using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AllowanceRepository
    {
        public List<string> GetFrequencyList()
        {
            return new List<string>()
            {
                Allowance.FREQUENCY_ONE_TIME,
                Allowance.FREQUENCY_DAILY,
                Allowance.FREQUENCY_SEMI_MONTHLY,
                Allowance.FREQUENCY_MONTHLY
            };
        }

        public async Task<IEnumerable<Allowance>> GetByEmployeeIncludesProductAsync(int? employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Allowances.Include(p => p.Product).Where(l => l.EmployeeID == employeeId).ToListAsync();
            }
        }

        public async Task<Allowance> GetByIdAsync(int? id)
        {
            using (var context = new PayrollContext())
            {
                return await context.Allowances.FirstOrDefaultAsync(l => l.RowID.Value == id.Value);
            }
        }

        public async Task<bool> CheckIfAlreadyUsed(int? id)
        {
            using (var context = new PayrollContext())
            {
                return await context.AllowanceItems.AnyAsync(a => a.AllowanceID == id);
            }
        }

        public async Task DeleteAsync(int? id)
        {
            using (var context = new PayrollContext())
            {
                var allowance = await GetByIdAsync(id);

                context.Remove(allowance);

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveManyAsync(int organizationID, int userID, List<Allowance> currentAllowances)
        {
            using (PayrollContext context = new PayrollContext())
            {
                foreach (var allowance in currentAllowances)
                {
                    await this.InternalSaveAsync(organizationID, userID, allowance, context);

                    await context.SaveChangesAsync();
                }
            }
        }

        internal async Task InternalSaveAsync(int organizationID, int userID, Allowance allowance, PayrollContext passedContext = null/* TODO Change to default(_) if this is not a reference type */)
        {
            // remove the product so it won't override the saving of ProductID
            var newAllowance = allowance.CloneJson();
            newAllowance.Product = null;

            newAllowance.OrganizationID = organizationID;

            // add or update the allowance
            if (passedContext == null)
            {
                using (PayrollContext newContext = new PayrollContext())
                {
                    await SaveAsyncFunction(userID, newAllowance, newContext);
                }
            }
            else
            {
                await SaveAsyncFunction(userID, newAllowance, passedContext);
            }

            // we used clone json at the top so the passed allowance
            // won't have the new RowID if the SaveAsync is Insert
            allowance.RowID = newAllowance.RowID;
        }

        public async Task SaveAsync(int organizationID, int userID, Allowance allowance/* TODO Change to default(_) if this is not a reference type */)
        {
            // add or update the allowance
            await InternalSaveAsync(organizationID, userID, allowance);
        }

        private async Task SaveAsyncFunction(int userID, Allowance newAllowance, PayrollContext context)
        {
            if (newAllowance.ProductID == null)
                throw new ArgumentException("Allowance type cannot be empty.");

            var product = await context.Products.Where(p => p.RowID.Value == newAllowance.ProductID.Value).FirstOrDefaultAsync();

            if (product == null)
                throw new ArgumentException("The selected allowance type no longer exists. Please close then reopen the form to view the latest data.");

            if (newAllowance.IsMonthly && !product.Fixed)
                throw new ArgumentException("Only fixed allowance type are allowed for Monthly allowances.");

            if (newAllowance.RowID == null)
                this.Insert(userID, newAllowance, context);
            else
                this.Update(userID, newAllowance, context);

            await context.SaveChangesAsync();
        }

        internal IQueryable<Allowance> GetAllowancesWithPayPeriodBaseQuery(int organizationID, PayrollContext context, DateTime payDateFrom, DateTime payDateTo)
        {
            // Retrieve all allowances whose begin and end date spans the cutoff dates.
            return context.Allowances.Include(a => a.Product).Where(a => a.OrganizationID.Value == organizationID).Where(a => a.EffectiveStartDate <= payDateTo).Where(a => a.EffectiveEndDate == null ? true : payDateFrom <= a.EffectiveEndDate.Value);
        }

        public async Task<ICollection<Allowance>> GetByPayPeriodWithProduct(int organizationID, DateTime payDateFrom, DateTime payDateTo)
        {
            using (var context = new PayrollContext())
            {
                return await GetAllowancesWithPayPeriodBaseQuery(organizationID,
                                                            context,
                                                            payDateFrom: payDateFrom,
                                                            payDateTo: payDateTo).
                                                            ToListAsync();
            }
        }

        public async Task<Allowance> GetEmployeeEcola(int employeeId, int organizationID, DateTime payDateFrom, DateTime payDateTo)
        {
            using (var context = new PayrollContext())
            {
                return await GetAllowancesWithPayPeriodBaseQuery(organizationID,
                                                            context,
                                                            payDateFrom: payDateFrom,
                                                            payDateTo: payDateTo).
                                    Where(a => a.EmployeeID.Value == employeeId).
                                    Where(a => a.Product.PartNo.ToLower() == ProductConstant.ECOLA).
                                    FirstOrDefaultAsync();
            }
        }

        private void Insert(int userID, Allowance allowance, PayrollContext context)
        {
            allowance.CreatedBy = userID;

            context.Allowances.Add(allowance);
        }

        private void Update(int userID, Allowance allowance, PayrollContext context)
        {
            allowance.LastUpdBy = userID;

            context.Allowances.Attach(allowance);
            context.Entry(allowance).State = EntityState.Modified;
        }
    }
}