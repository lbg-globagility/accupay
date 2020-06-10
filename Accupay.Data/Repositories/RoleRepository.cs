using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class RoleRepository
    {
        private readonly PayrollContext _context;

        public RoleRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<AspNetRole> GetById(Guid roleId)
        {
            var role = await _context.Roles.Include(t => t.RolePermissions)
                .FirstOrDefaultAsync(t => t.Id == roleId);

            return role;
        }

        public async Task<ICollection<AspNetRole>> GetAll()
        {
            var roles = await _context.Roles.Include(t => t.RolePermissions)
                .ToListAsync();

            return roles;
        }

        public async Task<AspNetRole> GetByUserAndOrganization(Guid userId, int organizationId)
        {
            var userRole = await _context.UserRoles
                .Where(t => t.UserId == userId && t.OrganizationId == organizationId)
                .FirstOrDefaultAsync();

            if (userRole is null) return null;

            var role = await _context.Roles
                .Include(t => t.RolePermissions)
                    .ThenInclude(t => t.Permission)
                .FirstOrDefaultAsync(t => t.Id == userRole.RoleId);

            return role;
        }

        public async Task<ICollection<UserRole>> GetUserRoles(int organizationId)
        {
            var userRoles = await _context.UserRoles
                .Where(t => t.OrganizationId == organizationId)
                .ToListAsync();

            return userRoles;
        }

        public async Task UpdateUserRoles(ICollection<UserRole> added,
                                          ICollection<UserRole> deleted)
        {
            _context.UserRoles.RemoveRange(deleted);

            foreach (var userRole in added)
            {
                _context.UserRoles.Add(userRole);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<AspNetRole> roles, int total)> List(PageOptions options, int clientId)
        {
            var query = _context.Roles
                .Where(t => t.ClientId == clientId);

            var roles = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (roles, total);
        }
    }
}
