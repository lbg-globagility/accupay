using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PositionRepository
    {
        private readonly PayrollContext _context;

        public PositionRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int positionId)
        {
            var position = await GetByIdAsync(positionId);

            _context.Remove(position);

            await _context.SaveChangesAsync();
        }

        public async Task<Position> SaveAsync(Position position, int organizationId, int divisionId)
        {
            // divisionId is passed as an additional check to have a divisionId that is not null
            position.DivisionID = divisionId;

            if (await GetByNameBaseQuery(organizationId, position.Name).AnyAsync())
                throw new ArgumentException("Position name already exists!");

            if (position.RowID == null)
            {
                _context.Positions.Add(position);
            }
            else
            {
                _context.Entry(position).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            var newPosition = await _context.Positions.
                                    FirstOrDefaultAsync(p => p.RowID == position.RowID);

            if (newPosition == null)
                throw new ArgumentException("There was a problem inserting the new position. Please try again.");

            return newPosition;
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Position> GetByIdAsync(int positionId)
        {
            return await _context.Positions.
                            Where(p => p.RowID == positionId).
                            FirstOrDefaultAsync();
        }

        public async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            return await _context.Positions.
                            Include(p => p.Division).
                            Where(p => p.RowID == positionId).
                            FirstOrDefaultAsync();
        }

        public async Task<Position> GetByNameAsync(int organizationId, string positionName)
        {
            return await GetByNameBaseQuery(organizationId, positionName).
                            FirstOrDefaultAsync();
        }

        private IQueryable<Position> GetByNameBaseQuery(int organizationId, string positionName)
        {
            return _context.Positions.
                        Where(p => p.OrganizationID == organizationId).
                        Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase());
        }

        public async Task<List<Position>> GetAllByNameAsync(string positionName)
        {
            return await _context.Positions.
                Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase()).
                ToListAsync();
        }

        public async Task<Position> GetByNameOrCreateAsync(string positionName, int organizationId, int userId, int divisionId)
        {
            var existingPosition = await GetByNameAsync(organizationId, positionName);

            if (existingPosition != null) return existingPosition;

            var position = new Position()
            {
                Name = positionName,
                OrganizationID = organizationId,
                CreatedBy = userId
            };

            return await SaveAsync(position: position,
                                    organizationId: organizationId,
                                    divisionId: divisionId);
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Position>> GetAllAsync(int organizationId)
        {
            return await _context.Positions.
                Where(p => p.OrganizationID == organizationId).
                ToListAsync();
        }

        #endregion List of entities

        #endregion Queries
    }
}