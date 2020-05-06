using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class SocialSecurityBracketRepository
    {
        public IEnumerable<SocialSecurityBracket> GetAll()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.SocialSecurityBrackets.ToList();
            }
        }

        public async Task<IEnumerable<SocialSecurityBracket>> GetByTimePeriodAsync(DateTime taxEffectivityDate)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.SocialSecurityBrackets.
                                Where(x => taxEffectivityDate >= x.EffectiveDateFrom).
                                Where(x => taxEffectivityDate <= x.EffectiveDateTo).
                                ToListAsync();
            }
        }
    }
}