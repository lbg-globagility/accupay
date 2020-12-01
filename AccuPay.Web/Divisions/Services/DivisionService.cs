using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Divisions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Divisions
{
    public class DivisionService
    {
        private readonly DivisionDataService _dataService;
        private readonly DivisionRepository _repository;
        private readonly ICurrentUser _currentUser;

        public DivisionService(DivisionDataService service, DivisionRepository repository, ICurrentUser currentuser)
        {
            _dataService = service;
            _repository = repository;
            _currentUser = currentuser;
        }

        public async Task<PaginatedList<DivisionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository
            var paginatedList = await _repository.List(options, _currentUser.OrganizationId, searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<DivisionDto> GetById(int id)
        {
            var division = await _repository.GetByIdWithParentAsync(id);

            return ConvertToDto(division);
        }

        internal async Task<ActionResult<DivisionDto>> Create(CreateDivisionDto dto)
        {
            var division = Division.NewDivision(
                organizationId: _currentUser.OrganizationId,
                userId: _currentUser.UserId);

            ApplyChanges(dto, division);

            await _dataService.SaveAsync(division);

            return ConvertToDto(division);
        }

        internal async Task<DivisionDto> Update(int id, UpdateDivisionDto dto)
        {
            var division = await _repository.GetByIdWithParentAsync(id);
            if (division == null) return null;

            division.LastUpdBy = _currentUser.UserId;

            ApplyChanges(dto, division);

            await _dataService.SaveAsync(division);

            return ConvertToDto(division);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                divisionId: id,
                changedByUserId: _currentUser.UserId);
        }

        internal IEnumerable<string> GetTypes()
        {
            return _repository.GetDivisionTypeList();
        }

        internal async Task<IEnumerable<DivisionDto>> GetAllParents()
        {
            IEnumerable<Division> parents = await _repository.GetAllParentsAsync(_currentUser.OrganizationId);

            var dtos = parents.Select(x => ConvertToDto(x));

            return dtos;
        }

        internal async Task<IEnumerable<string>> GetSchedules()
        {
            return await _dataService.GetSchedulesAsync();
        }

        private static DivisionDto ConvertToDto(Division division)
        {
            if (division == null) return null;

            return new DivisionDto()
            {
                Id = division.RowID.Value,
                Name = division.Name,
                ParentId = division.ParentDivisionID,
                ParentName = division.ParentDivision?.Name,
                AutomaticOvertimeFiling = division.AutomaticOvertimeFiling,
                DivisionType = division.DivisionType,
                WorkDaysPerYear = division.WorkDaysPerYear,
                WithholdingTaxSchedule = division.WithholdingTaxSchedule,
                PagIBIGDeductionSchedule = division.PagIBIGDeductionSchedule,
                SssDeductionSchedule = division.SssDeductionSchedule,
                PhilHealthDeductionSchedule = division.PhilHealthDeductionSchedule
            };
        }

        private void ApplyChanges(CrudDivisionDto dto, Division division)
        {
            division.Name = dto.Name;
            division.DivisionType = dto.DivisionType;
            division.ParentDivisionID = dto.ParentId;
            division.WorkDaysPerYear = dto.WorkDaysPerYear;
            division.AutomaticOvertimeFiling = dto.AutomaticOvertimeFiling;
            division.PhilHealthDeductionSchedule = dto.PhilHealthDeductionSchedule;
            division.SssDeductionSchedule = dto.SssDeductionSchedule;
            division.PagIBIGDeductionSchedule = dto.PagIBIGDeductionSchedule;
            division.WithholdingTaxSchedule = dto.WithholdingTaxSchedule;
        }
    }
}
