using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PositionRepository : SavableRepository<Position>, IPositionRepository
    {
        private readonly IDivisionRepository _divisionRepository;

        public PositionRepository(PayrollContext context, IDivisionRepository divisionRepository) : base(context)
        {
            _divisionRepository = divisionRepository;
        }

        protected override void DetachNavigationProperties(Position entity)
        {
            if (entity.Division != null)
            {
                _context.Entry(entity.Division).State = EntityState.Detached;
            }

            if (entity.JobLevel != null)
            {
                _context.Entry(entity.JobLevel).State = EntityState.Detached;
            }
        }

        #region Queries

        #region Single entity

        public async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            return await _context.Positions
                .Include(p => p.Division)
                .Where(p => p.RowID == positionId)
                .FirstOrDefaultAsync();
        }

        public async Task<Position> GetByNameAsync(int organizationId, string positionName)
        {
            return await CreateBaseQueryByName(organizationId, positionName)
                .FirstOrDefaultAsync();
        }

        public async Task<Position> GetByNameWithDivisionAsync(int organizationId, string positionName)
        {
            return await CreateBaseQueryByName(organizationId, positionName)
                .Include(p => p.Division)
                .FirstOrDefaultAsync();
        }

        private IQueryable<Position> CreateBaseQueryByName(int organizationId, string positionName)
        {
            return _context.Positions
                .AsNoTracking()
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Name.Trim().ToLower() == positionName.ToTrimmedLowerCase());
        }

        public async Task<Position> GetFirstPositionAsync()
        {
            return await _context.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<Position>> GetAllAsync(int organizationId)
        {
            return await _context.Positions
                .Where(p => p.OrganizationID == organizationId)
                .ToListAsync();
        }

        public async Task<PaginatedList<Position>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Positions
                .Include(x => x.Division)
                .Where(x => x.OrganizationID == organizationId)
                .OrderBy(x => x.Division.Name)
                .ThenBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, searchTerm) ||
                    EF.Functions.Like(x.Division.Name, searchTerm));
            }

            var positions = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Position>(positions, count);
        }

        #endregion List of entities

        #endregion Queries

        #region CRUD

        public async Task<List<Position>> CreateManyAsync(List<string> jobNames, int organizationId, int userId)
        {
            var division = await _divisionRepository.GetOrCreateDefaultDivisionAsync(organizationId, userId);

            var jobs = new List<Position>();
            foreach (var jobName in jobNames)
            {
                jobs.Add(new Position()
                {
                    Name = jobName,
                    DivisionID = division.RowID,
                    OrganizationID = organizationId,
                    CreatedBy = userId
                });
            }

            if (jobs.Any()) await SaveManyAsync(jobs);

            return jobs;
        }

        #endregion CRUD
    }
}
