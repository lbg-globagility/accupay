using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class RoleRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public RoleRepository(PayrollContext context)
        {
            _context = context;
        }

        #region Save

        public async Task CreateAsync(AspNetRole role)
        {
            _context.Entry(role).State = EntityState.Added;
            role.RolePermissions.ToList().ForEach(t => _context.Entry(t).State = EntityState.Added);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AspNetRole role)
        {
            _context.Entry(role).State = EntityState.Modified;
            role.RolePermissions.ToList().ForEach(t =>
            {
                if (IsNewEntity(t.Id))
                    _context.Entry(t).State = EntityState.Added;
                else
                    _context.Entry(t).State = EntityState.Modified;
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AspNetRole role)
        {
            _context.Remove(role);
            await _context.SaveChangesAsync();
        }

        #endregion Save

        #region User Roles

        public async Task UpdateUserRolesAsync(
            ICollection<UserRole> added,
            ICollection<UserRole> deleted)
        {
            _context.UserRoleTable.RemoveRange(deleted);

            foreach (var userRole in added)
            {
                _context.UserRoleTable.Add(userRole);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get user roles by organization.
        /// </summary>
        /// <param name="organizationId">The Id of the organization.</param>
        /// <returns></returns>
        public async Task<ICollection<UserRole>> GetUserRoles(int organizationId)
        {
            return await BaseGetUserRoles(organizationId);
        }

        /// <summary>
        /// Get ALL user roles.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<UserRole>> GetUserRoles()
        {
            return await BaseGetUserRoles();
        }

        private async Task<ICollection<UserRole>> BaseGetUserRoles(int? organizationId = null)
        {
            var query = _context.UserRoles
                .AsNoTracking();

            if (organizationId != null)
                query = query.Where(t => t.OrganizationId == organizationId);

            return await query.ToListAsync();
        }

        #endregion User Roles

        public async Task<AspNetRole> GetById(int roleId)
        {
            var role = await _context.Roles
                .Include(t => t.RolePermissions)
                .FirstOrDefaultAsync(t => t.Id == roleId);

            return role;
        }

        public async Task<AspNetRole> GetByUserAndOrganization(int userId, int organizationId)
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

        public async Task<(ICollection<AspNetRole> roles, int total)> List(PageOptions options, int clientId)
        {
            var query = _context.Roles
                .AsNoTracking()
                .Where(t => t.ClientId == clientId);

            var roles = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (roles, total);
        }

        /// <summary>
        /// Check for duplicate role name.
        /// </summary>
        /// <param name="name">The name of the role to be checked.</param>
        /// <param name="excludeId">If not null, checks for duplicate role name that has an Id that is not equal to excludeId.</param>
        /// <returns></returns>
        public async Task<bool> CheckForDuplicateAsync(string name, int? excludeId = null)
        {
            var query = _context.Roles.Where(t => t.Name.Trim().ToLower() == name.ToTrimmedLowerCase());

            if (excludeId != null)
            {
                query = query.Where(t => t.Id != excludeId);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasUsersAsync(int roleId)
        {
            return await (
                from role in _context.Roles
                join userRole in _context.UserRoles
                on role.Id
                equals userRole.RoleId
                where role.Id == roleId
                select userRole.UserId
            ).AnyAsync();
        }
    }
}