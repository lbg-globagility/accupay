using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
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

        #region Save

        public async Task CreateAsync(AspNetUser user)
        {
            _context.Entry(user).State = EntityState.Added;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AspNetUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(AspNetUser user)
        {
            user.DeletedAt = DateTime.Now;
            await UpdateAsync(user);
        }

        #endregion Save

        public async Task<AspNetUser> GetByIdAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<AspNetUser> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<UserRoleData>> GetUserRolesAsync(int userId)
        {
            IQueryable<UserRoleData> query = UserRoleQueryHelper.GetBaseQuery(_context);

            return await query.Where(x => x.User.Id == userId).ToListAsync();
        }

        public async Task<(ICollection<AspNetUser> users, int total)> List(PageOptions options, int clientId, string searchTerm = "", bool includeDeleted = false)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(x => x.ClientId == clientId);

            if (includeDeleted == false)
            {
                query = query.Where(x => x.DeletedAt == null);
            }

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

        public async Task<ICollection<AspNetUser>> GetUsersWithoutImageAsync()
        {
            return await _context.Users
                .Include(x => x.OriginalImage)
                .Where(x => x.OriginalImageId == null)
                .ToListAsync();
        }
    }
}