using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class DivisionDataService : BaseOrganizationDataService<Division>
    {
        private const string UserActivityDivisionName = "Division";

        private const string UserActivityDivisionLocationName = "Division Location";

        private readonly DivisionRepository _divisionRepository;
        private readonly ListOfValueRepository _listOfValueRepository;

        public DivisionDataService(
            DivisionRepository divisionRepository,
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(divisionRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Division")
        {
            _divisionRepository = divisionRepository;
            _listOfValueRepository = listOfValueRepository;
        }

        #region Save

        public async override Task DeleteAsync(int divisionId, int currentlyLoggedInUserId)
        {
            var division = await _divisionRepository.GetByIdWithParentAsync(divisionId);

            if (division == null)
                throw new BusinessLogicException("Division does not exists.");

            // TODO: move this to repositories
            if (_context.AgencyFees.Any(a => a.DivisionID == divisionId))
                throw new BusinessLogicException("Division already has agency fees therefore cannot be deleted.");
            else if (_context.Divisions.Any(d => d.ParentDivisionID == divisionId))
                throw new BusinessLogicException("Division already has child divisions therefore cannot be deleted.");
            else if (_context.Positions.Any(p => p.DivisionID == divisionId))
                throw new BusinessLogicException("Division already has positions therefore cannot be deleted.");

            await base.DeleteAsync(
                id: divisionId,
                currentlyLoggedInUserId: currentlyLoggedInUserId);
        }

        protected override string GetUserActivityName(Division division) =>
            division.IsRoot ?
            UserActivityDivisionLocationName :
            UserActivityDivisionName;

        protected override string CreateUserActivitySuffixIdentifier(Division allowance) => string.Empty;

        protected override async Task SanitizeEntity(Division division, Division oldDivision, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: division,
                oldEntity: oldDivision,
                currentlyLoggedInUserId: changedByUserId);

            var doesExistQuery = _context.Divisions
                .Where(d => d.Name.Trim().ToLower() == division.Name.ToTrimmedLowerCase())
                .Where(d => d.ParentDivisionID == division.ParentDivisionID)
                .Where(d => d.OrganizationID == division.OrganizationID);

            if (division.IsNewEntity == false)
            {
                doesExistQuery = doesExistQuery.Where(d => division.RowID != d.RowID);
            }

            if (await doesExistQuery.AnyAsync())
            {
                if (division.IsRoot)
                    throw new BusinessLogicException($"Division location already exists.");
                else
                    throw new BusinessLogicException($"Division name already exists under the selected division location.");
            }
        }

        #endregion Save

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int changedByUserId)
        {
            var divisionRepository = new DivisionRepository(_context);
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
                        defaultParentDivision = Division.NewDivision(organizationId);

                        defaultParentDivision.Name = Division.DefaultLocationName;
                        defaultParentDivision.ParentDivisionID = null;

                        await SanitizeEntity(defaultParentDivision, null, changedByUserId);
                        await divisionRepository.SaveAsync(defaultParentDivision);
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
                        defaultDivision = Division.NewDivision(organizationId);

                        defaultDivision.Name = Division.DefaultDivisionName;
                        defaultDivision.ParentDivisionID = defaultParentDivision.RowID;

                        await SanitizeEntity(defaultDivision, null, changedByUserId);
                        await divisionRepository.SaveAsync(defaultDivision);
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

        public async Task<ICollection<string>> GetSchedulesAsync()
        {
            ICollection<ListOfValue> listOfValues = await _listOfValueRepository.GetDeductionSchedulesAsync();
            return listOfValues.Select(x => x.DisplayValue).ToList();
        }
    }
}
