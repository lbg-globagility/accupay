using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Divisions.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Divisions
{
    public class DivisionService
    {
        private readonly DivisionDataService _dataService;

        public DivisionService(DivisionDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<PaginatedList<DivisionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _dataService.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<DivisionDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<DivisionDto> GetById(int id)
        {
            var division = await _dataService.GetByIdAsync(id);

            return ConvertToDto(division);
        }

        internal async Task<ActionResult<DivisionDto>> Create(CreateDivisionDto dto)
        {
            // TODO: validations

            int organizationId = 2;
            int userId = 1;
            var division = new Division()
            {
                CreatedBy = userId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, division);

            await _dataService.SaveAsync(division, organizationId);

            return ConvertToDto(division);
        }

        internal IEnumerable<string> GetTypes()
        {
            return  _dataService.GetTypes();
        }

        internal async Task<IEnumerable<DivisionDto>> GetAllParents()
        {
            int organizationId = 2;
            IEnumerable<Division> parents = await _dataService.GettAllParentsAsync(organizationId);

            var dtos = parents.Select(x => ConvertToDto(x));

            return dtos;

        }

        internal async Task<IEnumerable<string>> GetSchedules()
        {
            return await _dataService.GetSchedulesAsync();
        }

        internal async Task<DivisionDto> Update(int id, UpdateDivisionDto dto)
        {
            // TODO: validations

            var division = await _dataService.GetByIdAsync(id);
            if (division == null) return null;

            int organizationId = 2;
            int userId = 1;
            division.LastUpdBy = userId;

            ApplyChanges(dto, division);

            await _dataService.SaveAsync(division, organizationId);

            return ConvertToDto(division);
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
