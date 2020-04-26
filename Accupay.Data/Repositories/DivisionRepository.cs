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
        public static readonly string DIVISION_TYPE_DEPARTMENT = "Department";
        public static readonly string DIVISION_TYPE_BRANCH = "Branch";
        public static readonly string DIVISION_TYPE_SUB_BRANCH = "Sub branch";

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

        public async Task<Division> SaveAsync(Division division, int organizationId, int userId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                Division existingDivision = await GetByNameAndParentDivisionAsync(division.Name,
                                                                                    parentDivisionId: division.ParentDivisionID.Value,
                                                                                    organizationId: organizationId);

                if (division.RowID == null)
                    Insert(division, existingDivision, context, userId);
                else
                    Update(division, existingDivision, context, userId);

                await context.SaveChangesAsync();

                var newDivision = await context.Divisions.
                                            FirstOrDefaultAsync(d => d.RowID == division.RowID);

                if (newDivision == null)
                    throw new ArgumentException("There was a problem inserting the new division location. Please try again.");

                return newDivision;
            }
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

        public async Task<Division> GetByNameAndParentDivisionAsync(string positionName, int parentDivisionId, int organizationId)
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