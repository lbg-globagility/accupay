using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DivisionRepository : SavableRepository<Division>
    {
        public const string DIVISION_TYPE_DEPARTMENT = "Department";
        public const string DIVISION_TYPE_BRANCH = "Branch";
        public const string DIVISION_TYPE_SUB_BRANCH = "Sub branch";

        public DivisionRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Division entity)
        {
            if (entity.ParentDivision != null)
            {
                _context.Entry(entity.ParentDivision).State = EntityState.Detached;
            }
        }

        #region Queries

        #region Single entity

        public async Task<Division> GetByIdWithParentAsync(int id)
        {
            return await _context.Divisions
                .Include(x => x.ParentDivision)
                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int userId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var defaultParentDivision = await _context.Divisions
                        .Where(x => x.OrganizationID == organizationId)
                        .Where(x => x.Name.Trim().ToLower() == Division.DefaultLocationName.ToTrimmedLowerCase())
                        .Where(x => x.IsRoot)
                        .FirstOrDefaultAsync();

                    if (defaultParentDivision == null)
                    {
                        defaultParentDivision = Division.NewDivision(
                            organizationId: organizationId,
                            userId: userId);

                        defaultParentDivision.Name = Division.DefaultLocationName;
                        defaultParentDivision.ParentDivisionID = null;
                        defaultParentDivision.LastUpdBy = userId;

                        //await SanitizeEntity(defaultParentDivision, null);
                        await SaveAsync(defaultParentDivision);
                        // querying the new default parent division from here can already
                        // get the new row data. This can replace the context.local in leaverepository
                    }
                    if (defaultParentDivision?.RowID == null)
                        throw new Exception("Cannot create default division location.");

                    var defaultDivision = await _context.Divisions
                        .Where(x => x.OrganizationID == organizationId)
                        .Where(x => x.Name.Trim().ToLower() == Division.DefaultDivisionName.ToTrimmedLowerCase())
                        .Where(x => x.ParentDivisionID == defaultParentDivision.RowID)
                        .FirstOrDefaultAsync();

                    if (defaultDivision == null)
                    {
                        defaultDivision = Division.NewDivision(
                            organizationId: organizationId,
                            userId: userId);

                        defaultDivision.Name = Division.DefaultDivisionName;
                        defaultDivision.ParentDivisionID = defaultParentDivision.RowID;
                        defaultDivision.LastUpdBy = userId;

                        //await SanitizeEntity(defaultDivision, null);
                        await SaveAsync(defaultDivision);
                    }

                    transaction.Commit();

                    return defaultDivision;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            return await _context.Divisions
                .Where(d => d.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<PaginatedList<Division>> List(
            PageOptions options,
            int organizationId,
            string searchTerm = "")
        {
            var query = _context.Divisions
                .Include(x => x.ParentDivision)
                .Where(x => x.OrganizationID == organizationId)
                .OrderBy(x => x.ParentDivision.Name)
                .ThenBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.ParentDivision.Name, searchTerm));
            }

            var divisions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Division>(divisions, count);
        }

        public async Task<ICollection<Division>> GetAllParentsAsync(int organizationId)
        {
            return await _context.Divisions
                .Where(d => d.OrganizationID == organizationId)
                .Where(d => d.IsRoot)
                .ToListAsync();
        }

        #endregion List of entities

        #region Others

        public List<string> GetDivisionTypeList()
        {
            return new List<string>()
            {
                DIVISION_TYPE_DEPARTMENT,
                DIVISION_TYPE_BRANCH,
                DIVISION_TYPE_SUB_BRANCH
            };
        }

        #endregion Others

        #endregion Queries
    }
}
