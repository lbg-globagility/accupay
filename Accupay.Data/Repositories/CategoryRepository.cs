using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class CategoryRepository
    {
        public async Task<int?> GetLoanTypeId(int organizationID)
        {
            return await GetCategoryId(organizationID, ProductConstant.LEAVE_TYPE_CATEGORY);
        }

        private async Task<int?> GetCategoryId(int organizationID, string categoryName)
        {
            using (var context = new PayrollContext())
            {
                var category = await context.Categories.
                                        Where(c => c.OrganizationID == organizationID).
                                        Where(c => c.CategoryName == categoryName).
                                        FirstOrDefaultAsync();

                return category?.RowID;
            }
        }

        public async Task<Category> GetByName(int organizationID, string categoryName)
        {
            using (var context = new PayrollContext())
            {
                var category = await context.Categories.
                    Where(c => c.OrganizationID == organizationID).
                    Where(c => c.CategoryName == categoryName).
                    FirstOrDefaultAsync();

                return category;
            }
        }

        public async Task<Category> GetById(int rowID)
        {
            using (var context = new PayrollContext())
            {
                var category = await context.Categories.
                    Where(c => c.RowID == rowID).
                    FirstOrDefaultAsync();

                return category;
            }
        }

        public async Task SaveManyAsync(int organizationID, List<Category> categories)
        {
            foreach (var category in categories)
            {
                category.OrganizationID = organizationID;
            }

            using (PayrollContext context = new PayrollContext())
            {
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        internal async Task InternalSaveAsync(int organizationID, Category category, PayrollContext passedContext = null/* TODO Change to default(_) if this is not a reference type */)
        {
            category.OrganizationID = organizationID;

            if (passedContext == null)
            {
                var newContext = new PayrollContext();

                using (newContext)
                {
                    await SaveAsyncFunction(category, newContext);
                }
            }
            else
                await SaveAsyncFunction(category, passedContext);
        }

        public async Task SaveAsync(int organizationID, Category category)
        {
            await this.InternalSaveAsync(organizationID, category);
        }

        private async Task SaveAsyncFunction(Category category, PayrollContext context)
        {
            if (category.RowID == 0)
            {
                context.Categories.Add(category);
            }
            else
                await UpdateAsync(category, context);

            await context.SaveChangesAsync();
        }

        private async Task UpdateAsync(Category category, PayrollContext context)
        {
            var currentCategory = await context.Categories.FirstOrDefaultAsync(l => l.RowID == category.RowID);

            if (currentCategory == null)
                return;

            currentCategory.CategoryName = category.CategoryName;
        }

        public async Task DeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var overtime = await GetById(id);

                context.Remove(overtime);

                await context.SaveChangesAsync();
            }
        }
    }
}