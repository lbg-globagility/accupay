using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PayFrequencyRepository
    {
        public async Task<IEnumerable<PayFrequency>> GetAllAsync()

        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PayFrequencies.ToListAsync();
            }
        }
    }
}