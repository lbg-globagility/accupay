using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
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

        internal async Task<Division> GetByIdWithParentAsync(int id)
        {
            return await _context.Divisions
                                .Include(x => x.ParentDivision)
                                .FirstOrDefaultAsync(l => l.RowID == id);
        }

        #endregion Single entity

        #region List of entities

        internal IEnumerable<Division> GetAll(int organizationId)
        {
            return _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            ToList();
        }

        internal async Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            return await _context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                ToListAsync();
        }

        internal async Task<PaginatedList<Division>> List(
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

        internal async Task<IEnumerable<Division>> GetAllParentsAsync(int organizationId)
        {
            return await _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            Where(d => d.IsRoot).
                            ToListAsync();
        }

        #endregion List of entities

        #region Others

        internal List<string> GetDivisionTypeList()
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