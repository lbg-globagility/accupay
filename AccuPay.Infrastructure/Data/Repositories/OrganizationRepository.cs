using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class OrganizationRepository : SavableRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Organization organization)
        {
            if (organization.Address != null)
            {
                _context.Entry(organization.Address).State = EntityState.Detached;
            }
        }

        public override async Task DeleteAsync(Organization organization)
        {
            organization = await _context.Organizations
                .Include(x => x.Categories)
                .ThenInclude(x => x.Products)
                .Where(x => x.RowID == organization.RowID)
                .FirstOrDefaultAsync();

            var positions = _context.Positions
                .Where(x => x.OrganizationID == organization.RowID);

            var divisionLocations = _context.Divisions
                .Where(x => x.OrganizationID == organization.RowID)
                .Where(x => x.IsRoot);

            var divisions = _context.Divisions
                .Where(x => x.OrganizationID == organization.RowID)
                .Where(x => !x.IsRoot);

            var payPeriods = _context.PayPeriods
                .Where(x => x.OrganizationID == organization.RowID);

            var listOfValues = _context.ListOfValues
                .Where(x => x.OrganizationID == organization.RowID);

            var views = _context.Views
                .Where(x => x.OrganizationID == organization.RowID);

            var auditTrails = _context.AuditTrails
                .Where(x => x.OrganizationID == organization.RowID);

            var userRoles = _context.UserRoles
                .Where(x => x.OrganizationId == organization.RowID);

            var userClaims = _context.UserClaims
                .Where(x => x.OrganizationId == organization.RowID);

            var userLogins = _context.UserLogins
                .Where(x => x.OrganizationId == organization.RowID);

            var userTokens = _context.UserTokens
                .Where(x => x.OrganizationId == organization.RowID);

            _context.Positions.RemoveRange(positions);
            _context.Divisions.RemoveRange(divisionLocations);
            _context.Divisions.RemoveRange(divisions);

            _context.PayPeriods.RemoveRange(payPeriods);

            _context.UserRoles.RemoveRange(userRoles);
            _context.UserClaims.RemoveRange(userClaims);
            _context.UserLogins.RemoveRange(userLogins);
            _context.UserTokens.RemoveRange(userTokens);

            _context.ListOfValues.RemoveRange(listOfValues);
            _context.Views.RemoveRange(views);
            _context.AuditTrails.RemoveRange(auditTrails);

            _context.Organizations.Remove(organization);

            await _context.SaveChangesAsync();
        }

        #region Queries

        public async Task<bool> CheckIfNameExistsAsync(string name, int? id)
        {
            var query = _context.Organizations
                .Where(x => x.Name.Trim().ToLower() == name.ToTrimmedLowerCase());

            if (id != null)
            {
                query = query.Where(x => x.RowID != id);
            }

            return await query.AnyAsync();
        }

        public async Task<Organization> GetByIdWithAddressAsync(int id)
        {
            return await _context.Organizations
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async Task<Organization> GetFirst(int clientId)
        {
            return await _context.Organizations
                .Where(o => o.ClientId == clientId)
                .Where(o => o.IsInActive == false)
                .FirstOrDefaultAsync();
        }

        public async Task<(ICollection<Organization> organizations, int total)> List(OrganizationPageOptions options, int clientId)
        {
            var query = _context.Organizations
                .AsNoTracking()
                .Where(o => o.IsInActive == false);

            if (options.HasClientId)
            {
                query = query.Where(t => t.ClientId == options.ClientId);
            }
            else
            {
                query = query.Where(t => t.ClientId == clientId);
            }

            var organizations = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (organizations, total);
        }

        public async Task<ICollection<UserRoleData>> GetUserRolesAsync(int organizationId)
        {
            IQueryable<UserRoleData> query = UserRoleQueryHelper.GetBaseQuery(_context);

            return await query.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }

        #endregion Queries
    }
}
