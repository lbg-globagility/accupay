using AccuPay.Data.Entities;
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

        #region CRUD

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

        public async Task<Division> SaveAsync(Division division, int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await SaveAsyncFunction(division, organizationId, context);
            }
        }

        private async Task<Division> SaveAsyncFunction(Division division, int organizationId, PayrollContext context)
        {
            Division existingDivision = await GetByNameAndParentDivisionAsync(division.Name,
                                                                            parentDivisionId: division.ParentDivisionID,
                                                                            organizationId: organizationId);

            if (division.RowID == null)
            {
                if (existingDivision != null)
                    throw new ArgumentException($"Division name already exists under {existingDivision.ParentDivision.Name }!");

                context.Divisions.Add(division);
            }
            else
            {
                if (existingDivision != null && division.RowID != existingDivision.RowID)
                    throw new ArgumentException($"Division name already exists under {existingDivision.ParentDivision.Name }!");

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
            using (var context = new PayrollContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var defaultParentDivision = await context.Divisions.
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

                            await SaveAsyncFunction(defaultParentDivision, organizationId, context);
                            // querying the new default parent division from here can already
                            // get the new row data. This can replace the context.local in leaverepository
                        }
                        if (defaultParentDivision?.RowID == null)
                            throw new Exception("Cannot create default division location.");

                        var defaultDivision = await context.Divisions.
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
                            await SaveAsyncFunction(defaultDivision, organizationId, context);
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

        public async Task<Division> GetByNameAndParentDivisionAsync(string positionName, int? parentDivisionId, int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                        Include(d => d.ParentDivision).
                                        Where(d => d.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase()).
                                        Where(d => d.ParentDivisionID == parentDivisionId).
                                        Where(d => d.OrganizationID == organizationId).
                                        FirstOrDefaultAsync();
            }
        }

        #endregion Single entity

        #region List of entities

        public IEnumerable<Division> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                ToList();
            }
        }

        public async Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                        Where(d => d.OrganizationID == organizationId).
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<Division>> GetAllParentsAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Divisions.
                                Where(d => d.OrganizationID == organizationId).
                                Where(d => d.IsRoot).
                                ToListAsync();
            }
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