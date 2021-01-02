using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PayrollContext _context;

        public CategoryRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByNameAsync(int organizationId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return null;

            var category = await _context.Categories.
                Where(c => c.OrganizationID == organizationId).
                Where(c => c.CategoryName.Trim().ToLower() ==
                            categoryName.ToTrimmedLowerCase()).
                FirstOrDefaultAsync();

            return category;
        }
    }
}
