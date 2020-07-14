using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class DivisionDataService : BaseSavableDataService<Division>
    {
        private readonly DivisionRepository _divisionRepository;
        private readonly ListOfValueRepository _listOfValueRepository;
        private readonly PayrollContext _context;

        public DivisionDataService(
            DivisionRepository divisionRepository,
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context) :

            base(divisionRepository,
                payPeriodRepository,
                entityDoesNotExistOnDeleteErrorMessage: "Division does not exists.")
        {
            _divisionRepository = divisionRepository;
            _listOfValueRepository = listOfValueRepository;
            _context = context;
        }

        #region CRUD

        public async override Task DeleteAsync(int divisionId)
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

            await _divisionRepository.DeleteAsync(division);
        }

        protected override async Task SanitizeEntity(Division division)
        {
            if (division.OrganizationID == null)
                throw new BusinessLogicException($"Organization is required.");

            var doesExistQuery = _context.Divisions
                                        .Where(d => d.Name.Trim().ToLower() ==
                                                division.Name.ToTrimmedLowerCase())
                                        .Where(d => d.ParentDivisionID == division.ParentDivisionID)
                                        .Where(d => d.OrganizationID == division.OrganizationID);

            if (IsNewEntity(division.RowID) == false)
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

        #endregion CRUD

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int userId)
        {
            var divisionRepository = new DivisionRepository(_context);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var defaultParentDivision = await _context.Divisions.
                                            Where(x => x.OrganizationID == organizationId).
                                            Where(x => x.Name.Trim().ToLower() == Division.DefaultLocationName.ToTrimmedLowerCase()).
                                            Where(x => x.IsRoot).
                                            FirstOrDefaultAsync();

                    if (defaultParentDivision == null)
                    {
                        defaultParentDivision = Division.NewDivision(
                                                            organizationId: organizationId,
                                                            userId: userId);

                        defaultParentDivision.Name = Division.DefaultLocationName;
                        defaultParentDivision.ParentDivisionID = null;

                        await SanitizeEntity(defaultParentDivision);
                        await divisionRepository.SaveAsync(defaultParentDivision);
                        // querying the new default parent division from here can already
                        // get the new row data. This can replace the context.local in leaverepository
                    }
                    if (defaultParentDivision?.RowID == null)
                        throw new Exception("Cannot create default division location.");

                    var defaultDivision = await _context.Divisions.
                                                    Where(x => x.OrganizationID == organizationId).
                                                    Where(x => x.Name.Trim().ToLower() == Division.DefaultDivisionName.ToTrimmedLowerCase()).
                                                    Where(x => x.ParentDivisionID == defaultParentDivision.RowID).
                                                    FirstOrDefaultAsync();

                    if (defaultDivision == null)
                    {
                        defaultDivision = Division.NewDivision(
                                                            organizationId: organizationId,
                                                            userId: userId);

                        defaultDivision.Name = Division.DefaultDivisionName;
                        defaultDivision.ParentDivisionID = defaultParentDivision.RowID;

                        await SanitizeEntity(defaultDivision);
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

        public async Task<Division> GetByIdWithParentAsync(int id)
        {
            return await _divisionRepository.GetByIdWithParentAsync(id);
        }

        public IEnumerable<Division> GetAll(int organizationId)
        {
            return _divisionRepository.GetAll(organizationId);
        }

        public Task<IEnumerable<Division>> GetAllAsync(int organizationId)
        {
            return _divisionRepository.GetAllAsync(organizationId);
        }

        public async Task<PaginatedList<Division>> List(PageOptions options, int organizationId, string searchTerm)
        {
            return await _divisionRepository.List(options, organizationId, searchTerm);
        }

        public async Task<IEnumerable<Division>> GetAllParentsAsync(int organizationId)
        {
            return await _divisionRepository.GetAllParentsAsync(organizationId);
        }

        public IEnumerable<string> GetTypes()
        {
            return _divisionRepository.GetDivisionTypeList();
        }

        public async Task<IEnumerable<string>> GetSchedulesAsync()
        {
            IEnumerable<ListOfValue> listOfValues = await _listOfValueRepository.GetDeductionSchedulesAsync();
            return listOfValues.Select(x => x.DisplayValue);
        }
    }
}