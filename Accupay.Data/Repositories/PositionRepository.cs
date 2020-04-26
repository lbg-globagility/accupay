using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionRepository
    {
        public async Task<List<Position>> GetAllAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                    Where(p => p.OrganizationID == organizationId).
                    ToListAsync();
            }
        }

        public async Task<Position> GetByIdAsync(int positionId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                                Where(p => p.RowID == positionId).
                                FirstOrDefaultAsync();
            }
        }

        public async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                                Include(p => p.Division).
                                Where(p => p.RowID == positionId).
                                FirstOrDefaultAsync();
            }
        }

        public async Task<Position> GetByNameAsync(int organizationId, string positionName)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Positions.
                    Where(p => p.OrganizationID == organizationId).
                    Where(p => p.Name.ToLower().Trim() == positionName.ToLower().Trim()).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<Position> GetByNameOrCreateAsync(string positionName, int organizationId, int userId)
        {
            var existingPosition = await GetByNameAsync(organizationId, positionName);

            if (existingPosition != null) return existingPosition;

            var position = new Position()
            {
                Name = positionName,
                OrganizationID = organizationId
                // Before Update Trigger will populate the DivisionID,
                // That should be coded in a service
            };

            return await SaveAsync(position, organizationId, userId);
        }

        public async Task<Position> SaveAsync(Position position, int organizationId, int userId)
        {
            position.Name = position.Name.Trim().ToLower();

            using (PayrollContext context = new PayrollContext())
            {
                Position existingPosition = await GetByNameAsync(organizationId, position.Name);

                if (position.RowID == null)
                    Insert(position, existingPosition, context, userId);
                else
                    Update(position, existingPosition, context, userId);

                await context.SaveChangesAsync();

                var newPosition = await context.Positions.
                                        FirstOrDefaultAsync(p => p.RowID == position.RowID);

                if (newPosition == null)
                    throw new ArgumentException("There was a problem inserting the new position. Please try again.");

                return newPosition;
            }
        }

        public async Task DeleteAsync(int positionId)
        {
            using (var context = new PayrollContext())
            {
                var position = await GetByIdAsync(positionId);

                context.Remove(position);

                await context.SaveChangesAsync();
            }
        }

        //private async Task<int?> GetCategoryId(string categoryName)
        //{
        //    var category = await _categoryRepository.GetByName(z_OrganizationID, categoryName);
        //    return category.RowID;
        //}

        private void Insert(Position position, Position existingPosition, PayrollContext context, int userId)
        {
            if (existingPosition != null)
                throw new ArgumentException("Position name already exists!");

            position.CreatedBy = userId;

            context.Positions.Add(position);
        }

        private void Update(Position position, Position existingPosition, PayrollContext context, int userId)
        {
            if (existingPosition != null && position.RowID != existingPosition.RowID)
                throw new ArgumentException("Position name already exists!");

            position.LastUpdBy = userId;

            context.Positions.Attach(position);
            context.Entry(position).State = EntityState.Modified;
        }
    }
}