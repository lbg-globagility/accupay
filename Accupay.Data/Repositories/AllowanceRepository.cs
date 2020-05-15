using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
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
        private readonly PayrollContext _context;

        public AllowanceRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int id)
        {
            var allowance = await GetByIdAsync(id);

            _context.Remove(allowance);

            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<Allowance> allowances)
        {
            foreach (var allowance in allowances)
            {
                await SaveWithContextAsync(allowance);

                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(Allowance allowance)
        {
            await SaveWithContextAsync(allowance, deferSave: false);
        }

        private async Task SaveWithContextAsync(Allowance allowance, bool deferSave = true)
        {
            // remove the product so it won't override the saving of ProductID
            // this is probably done because Product is used in front end that is why it has data
            // other repository has no need for this, maybe standardize this TODO:
            var newAllowance = allowance.CloneJson();
            newAllowance.Product = null;

            if (deferSave == false)
            {
                // TODO: refactor this code to always call SaveAsyncFunction
                await SaveAsyncFunction(newAllowance);
                await _context.SaveChangesAsync();
            }
            else
            {
                await SaveAsyncFunction(newAllowance);
            }

            // we used clone json at the top so the passed allowance
            // won't have the new RowID if the SaveAsync is Insert
            allowance.RowID = newAllowance.RowID;
        }

        private async Task SaveAsyncFunction(Allowance newAllowance)
        {
            if (newAllowance.ProductID == null)
                throw new ArgumentException("Allowance type cannot be empty.");

            var product = await _context.Products.
                                    Where(p => p.RowID == newAllowance.ProductID).
                                    FirstOrDefaultAsync();

            if (product == null)
                throw new ArgumentException("The selected allowance type no longer exists. Please close then reopen the form to view the latest data.");

            if (newAllowance.IsMonthly && !product.Fixed)
                throw new ArgumentException("Only fixed allowance type are allowed for Monthly allowances.");

            // add or update the allowance
            if (newAllowance.RowID == null)
                _context.Allowances.Add(newAllowance);
            else
                _context.Entry(newAllowance).State = EntityState.Modified;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Allowance> GetByIdAsync(int id)
        {
            return await _context.Allowances.FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Allowance> GetEmployeeEcolaAsync(int employeeId,
                                                        int organizationId,
                                                        TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod).

                                Where(a => a.EmployeeID == employeeId).
                                Where(a => a.Product.PartNo.ToLower() ==
                                            ProductConstant.ECOLA).
                                FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Allowance>> GetByEmployeeWithProductAsync(int employeeId)
        {
            return await _context.Allowances.
                            Include(p => p.Product).
                            Where(l => l.EmployeeID == employeeId).
                            ToListAsync();
        }

        public ICollection<Allowance> GetByPayPeriodWithProduct(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return CreateBaseQueryByTimePeriod(organizationId,
                                            timePeriod).
                                            ToList();
        }

        public async Task<ICollection<Allowance>> GetByPayPeriodWithProductAsync(int organizationId,
                                                                                TimePeriod timePeriod)
        {
            return await CreateBaseQueryByTimePeriod(organizationId,
                                                    timePeriod).
                                                    ToListAsync();
        }

        #endregion List of entities

        #region Others

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

        public async Task<bool> CheckIfAlreadyUsed(int id)
        {
            return await _context.AllowanceItems.AnyAsync(a => a.AllowanceID == id);
        }

        public async Task<bool> CheckIfAlreadyUsed(string allowanceName)
        {
            return await _context.AllowanceItems.
                                Include(x => x.Allowance).
                                Include(x => x.Allowance.Product).
                                Where(x => x.Allowance.Product.PartNo == allowanceName).
                                AnyAsync();
        }

        #endregion Others

        #endregion Queries

        #region Private helper methods

        private IQueryable<Allowance> CreateBaseQueryByTimePeriod(int organizationId,
                                                                TimePeriod timePeriod)
        {
            return _context.Allowances.
                    Include(a => a.Product).
                    Where(a => a.OrganizationID == organizationId).
                    Where(a => a.EffectiveStartDate <= timePeriod.End).
                    Where(a => a.EffectiveEndDate == null ? true : timePeriod.Start <= a.EffectiveEndDate);
        }

        #endregion Private helper methods
    }
}