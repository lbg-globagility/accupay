using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class CategoryRepository
    {
        public async Task<Category> GetByName(int organizationId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return null;

            using (var context = new PayrollContext())
            {
                var category = await context.Categories.
                    Where(c => c.OrganizationID == organizationId).
                    Where(c => c.CategoryName.Trim().ToLower() == categoryName.ToTrimmedLowerCase()).
                    FirstOrDefaultAsync();

                return category;
            }
        }
    }
}