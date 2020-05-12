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
        private readonly PayrollContext _context;

        public SocialSecurityBracketRepository(PayrollContext context)
        {
            _context = context;
        }

        public IEnumerable<SocialSecurityBracket> GetAll()
        {
            return _context.SocialSecurityBrackets.ToList();
        }

        public async Task<IEnumerable<SocialSecurityBracket>> GetByTimePeriodAsync(DateTime taxEffectivityDate)
        {
            return await _context.SocialSecurityBrackets.
                            Where(x => taxEffectivityDate >= x.EffectiveDateFrom).
                            Where(x => taxEffectivityDate <= x.EffectiveDateTo).
                            ToListAsync();
        }
    }
}