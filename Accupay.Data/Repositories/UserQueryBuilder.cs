using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class UserQueryBuilder
    {
        private PayrollContext _context;
        private IQueryable<User> _query;

        public UserQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.OldUsers;
        }

        public UserQueryBuilder ById(int id)
        {
            _query = _query.Where(u => u.RowID == id);
            return this;
        }

        public UserQueryBuilder ByAspNetUserId(Guid aspNetUserId)
        {
            _query = _query.Where(u => u.AspNetUserId == aspNetUserId);
            return this;
        }

        public UserQueryBuilder ByUsername(string username)
        {
            _query = _query.Where(u => u.Username.Trim().ToLower() == username.ToTrimmedLowerCase());
            return this;
        }

        public UserQueryBuilder IsActive()
        {
            _query = _query.Where(u => u.IsActive);
            return this;
        }

        public UserQueryBuilder IncludePosition()
        {
            _query = _query.Include(u => u.Position);
            return this;
        }

        public async Task<ICollection<User>> ToListAsync()
        {
            return await _query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> FirstOrDefaultAsync()
        {
            return await _query
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await _query
                .AsNoTracking()
                .AnyAsync();
        }
    }
}