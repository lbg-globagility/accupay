using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SocialSecurityBracketRepository : ISocialSecurityBracketRepository
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
