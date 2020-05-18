using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Notisphere.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<(ICollection<AspNetUser>, int)> List(PageOptions options, string searchTerm = "")
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(u =>
                    EF.Functions.Like(u.FirstName, searchTerm) ||
                    EF.Functions.Like(u.LastName, searchTerm) ||
                    EF.Functions.Like(u.Email, searchTerm));
            }

            var users = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return (users, count);
        }
    }
}
