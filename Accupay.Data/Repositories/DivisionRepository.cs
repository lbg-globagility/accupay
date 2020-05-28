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
    public class DivisionRepository
    {
        public const string DIVISION_TYPE_DEPARTMENT = "Department";
        public const string DIVISION_TYPE_BRANCH = "Branch";
        public const string DIVISION_TYPE_SUB_BRANCH = "Sub branch";

        public const string DEFAULT_LOCATION = "Default Location";
        public const string DEFAULT_DIVISION = "Default Division";

        private readonly PayrollContext _context;

        public DivisionRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int divisionId)
        {
            if (_context.AgencyFees.Any(a => a.DivisionID == divisionId))
                throw new ArgumentException("Division already has agency fees therefore cannot be deleted.");
            else if (_context.Divisions.Any(d => d.ParentDivisionID == divisionId))
                throw new ArgumentException("Division already has child divisions therefore cannot be deleted.");
            else if (_context.Positions.Any(p => p.DivisionID == divisionId))
                throw new ArgumentException("Division already has positions therefore cannot be deleted.");

            var division = await _context.Divisions.
                            FirstOrDefaultAsync(d => d.RowID == divisionId);

            _context.Remove(division);

            await _context.SaveChangesAsync();
        }

        public async Task<Division> SaveAsync(Division division, int organizationId)
        {
            return await SaveAsyncFunction(division, organizationId, _context);
        }

        private async Task<Division> SaveAsyncFunction(Division division, int organizationId, PayrollContext context)
        {
            if (await _context.Divisions.
                            Include(d => d.ParentDivision).
                            Where(l => division.RowID == null ? true : division.RowID != l.RowID).
                            Where(d => d.Name.Trim().ToLower() ==
                                        division.Name.ToTrimmedLowerCase()).
                            Where(d => d.ParentDivisionID == division.ParentDivisionID).
                            Where(d => d.OrganizationID == organizationId).
                            AnyAsync())
            {
                if (division.IsRoot)
                {
                    throw new ArgumentException($"Division location already exists.");
                }
                else
                {
                    throw new ArgumentException($"Division name already exists under the selected division location.");
                }
            }

            if (division.RowID == null)
            {
                context.Divisions.Add(division);
            }
            else
            {
                context.Entry(division).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();

            var newDivision = await context.Divisions.
                                        FirstOrDefaultAsync(d => d.RowID == division.RowID);

            if (newDivision == null)
                throw new ArgumentException("There was a problem inserting the new division. Please try again.");

            return newDivision;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int userId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var defaultParentDivision = await _context.Divisions.
                                            Where(x => x.OrganizationID == organizationId).
                                            Where(x => x.Name.Trim().ToLower() == DEFAULT_LOCATION.ToTrimmedLowerCase()).
                                            Where(x => x.IsRoot).
                                            FirstOrDefaultAsync();

                    if (defaultParentDivision == null)
                    {
                        defaultParentDivision = Division.CreateEmptyDivision(
                                                            organizationId: organizationId,
                                                            userId: userId);

                        defaultParentDivision.Name = DEFAULT_LOCATION;
                        defaultParentDivision.ParentDivisionID = null;

                        await SaveAsyncFunction(defaultParentDivision, organizationId, _context);
                        // querying the new default parent division from here can already
                        // get the new row data. This can replace the context.local in leaverepository
                    }
                    if (defaultParentDivision?.RowID == null)
                        throw new Exception("Cannot create default division location.");

                    var defaultDivision = await _context.Divisions.
                                                    Where(x => x.OrganizationID == organizationId).
                                                    Where(x => x.Name.Trim().ToLower() == DEFAULT_DIVISION.ToTrimmedLowerCase()).
                                                    Where(x => x.IsRoot == false).
                                                    FirstOrDefaultAsync();

                    if (defaultDivision == null)
                    {
                        defaultDivision = Division.CreateEmptyDivision(
                                                            organizationId: organizationId,
                                                            userId: userId);

                        defaultDivision.Name = DEFAULT_DIVISION;
                        defaultDivision.ParentDivisionID = defaultParentDivision.RowID;
                        await SaveAsyncFunction(defaultDivision, organizationId, _context);
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

        public IEnumerable<Division> GetAll(int organizationId)
        {
            return _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            ToList();
        }

        public async Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            return await _context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                ToListAsync();
        }

        internal async Task<PaginatedListResult<Division>> GetPaginatedListAsync(PageOptions options, int organizationId, bool isRoot, string searchTerm = "")
        {
            var query = _context.Divisions
                                .Include(x => x.ParentDivision)
                                .Where(x => x.OrganizationID == organizationId)
                                .OrderBy(x => x.ParentDivision.Name)
                                .ThenBy(x => x.Name)
                                .AsQueryable();

            if (isRoot)
            {
                query = query.Where(x => x.ParentDivisionID == null);
            }
            else
            {
                query = query.Where(x => x.ParentDivisionID != null);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.ParentDivision.Name, searchTerm));
            }

            var divisions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Division>(divisions, count);
        }

        public async Task<IEnumerable<Division>> GetAllParentsAsync(int organizationId)
        {
            return await _context.Divisions.
                            Where(d => d.OrganizationID == organizationId).
                            Where(d => d.IsRoot).
                            ToListAsync();
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