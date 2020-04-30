using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PhilHealthBracketRepository
    {
        public IEnumerable<PhilHealthBracket> GetAll()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PhilHealthBrackets.ToList();
            }
        }

        public async Task<IEnumerable<PhilHealthBracket>> GetAllAsync()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PhilHealthBrackets.ToListAsync();
            }
        }
    }
}