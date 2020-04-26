using AccuPay.Data.Entities;
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

        public List<string> GetDivisionTypeList()
        {
            return new List<string>()
            {
                DIVISION_TYPE_DEPARTMENT,
                DIVISION_TYPE_BRANCH,
                DIVISION_TYPE_SUB_BRANCH
            };
        }

        public IEnumerable<Division> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                ToList();
            }
        }

        public async Task<List<Division>> GetAllAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                        Where(d => d.OrganizationID == organizationId).
                                        ToListAsync();
            }
        }

        public async Task<List<Division>> GetAllParentsAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                Where(d => d.IsRoot).
                                ToListAsync();
            }
        }

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int userId)
        {
            using (var context = new PayrollContext(PayrollContext.DbCommandConsoleLoggerFactory))
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var defaultParentDivision = await context.Divisions.
                                                Where(x => x.OrganizationID == organizationId).
                                                Where(x => x.Name.Trim().ToLower() == DEFAULT_LOCATION.Trim().ToLower()).
                                                Where(x => x.IsRoot).
                                                FirstOrDefaultAsync();

                        if (defaultParentDivision == null)
                        {
                            defaultParentDivision = Division.CreateEmptyDivision(organizationId);
                            defaultParentDivision.Name = DEFAULT_LOCATION;
                            defaultParentDivision.ParentDivisionID = null;

                            await SaveAsyncFunction(defaultParentDivision, organizationId, userId, context);
                            // querying the new default parent division from here can already
                            // get the new row data. This can replace the context.local in leaverepository
                        }
                        if (defaultParentDivision?.RowID == null)
                            throw new Exception("Cannot create default division location.");

                        var defaultDivision = await context.Divisions.
                                                        Where(x => x.OrganizationID == organizationId).
                                                        Where(x => x.Name.Trim().ToLower() == DEFAULT_DIVISION.Trim().ToLower()).
                                                        Where(x => x.IsRoot == false).
                                                        FirstOrDefaultAsync();

                        if (defaultDivision == null)
                        {
                            defaultDivision = Division.CreateEmptyDivision(organizationId);
                            defaultDivision.Name = DEFAULT_DIVISION;
                            defaultDivision.ParentDivisionID = defaultParentDivision.RowID.Value;
                            await SaveAsyncFunction(defaultDivision, organizationId, userId, context);
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
        }

        public async Task<Division> SaveAsync(Division division, int organizationId, int userId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await SaveAsyncFunction(division, organizationId, userId, context);
            }
        }

        private async Task<Division> SaveAsyncFunction(Division division, int organizationId, int userId, PayrollContext context)
        {
            Division existingDivision = await GetByNameAndParentDivisionAsync(division.Name,
                                                                                                parentDivisionId: division.ParentDivisionID,
                                                                                                organizationId: organizationId);

            if (division.RowID == null)
                Insert(division, existingDivision, context, userId);
            else
                Update(division, existingDivision, context, userId);

            await context.SaveChangesAsync();

            var newDivision = await context.Divisions.
                                        FirstOrDefaultAsync(d => d.RowID == division.RowID);

            if (newDivision == null)
                throw new ArgumentException("There was a problem inserting the new division. Please try again.");

            return newDivision;
        }

        public async Task DeleteAsync(int divisionId)
        {
            using (var context = new PayrollContext())
            {
                if (context.AgencyFees.Any(a => a.DivisionID == divisionId))
                    throw new ArgumentException("Division already has agency fees therefore cannot be deleted.");
                else if (context.Divisions.Any(d => d.ParentDivisionID == divisionId))
                    throw new ArgumentException("Division already has child divisions therefore cannot be deleted.");
                else if (context.Positions.Any(p => p.DivisionID == divisionId))
                    throw new ArgumentException("Division already has positions therefore cannot be deleted.");

                var division = await context.Divisions.
                                                FirstOrDefaultAsync(d => d.RowID == divisionId);

                context.Remove(division);

                await context.SaveChangesAsync();
            }
        }

        public async Task<Division> GetByNameAndParentDivisionAsync(string positionName, int? parentDivisionId, int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                        Include(d => d.ParentDivision).
                                        Where(d => d.Name.ToLower().Trim() == positionName.ToLower().Trim()).
                                        Where(d => d.ParentDivisionID == parentDivisionId).
                                        Where(d => d.OrganizationID == organizationId).
                                        FirstOrDefaultAsync();
            }
        }

        private void Insert(Division division, Division existingDivision, PayrollContext context, int userId)
        {
            if (existingDivision != null)
                throw new ArgumentException($"Division name already exists under {existingDivision.ParentDivision.Name }!");

            division.CreatedBy = userId;

            context.Divisions.Add(division);
        }

        private void Update(Division division, Division existingDivision, PayrollContext context, int userId)
        {
            if (existingDivision != null && division.RowID != existingDivision.RowID)
                throw new ArgumentException($"Division name already exists under {existingDivision.ParentDivision.Name }!");

            division.LastUpdBy = userId;

            context.Divisions.Attach(division);
            context.Entry(division).State = EntityState.Modified;
        }
    }
}