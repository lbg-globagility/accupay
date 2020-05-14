using AccuPay.Data.Entities;
using System;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class AspNetUserRepository
    {
        private readonly PayrollContext _context;

        public AspNetUserRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<AspNetUser> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
