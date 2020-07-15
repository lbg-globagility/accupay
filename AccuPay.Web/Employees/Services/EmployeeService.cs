using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports.Employees;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Files.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Employees.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly GenerateDefaultImageService _generateDefaultImageService;
        private readonly IFilesystem _filesystem;
        private readonly FileRepository _fileRepository;
        private readonly EmployeeDataService _service;
        private readonly EmployeeImportParser _importParser;

        public EmployeeService(EmployeeRepository employeeRepository,
            ICurrentUser currentUser,
            GenerateDefaultImageService generateDefaultImageService,
            IFilesystem filesystem,
            FileRepository fileRepository,
            EmployeeDataService service,
            EmployeeImportParser importParser)
        {
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _generateDefaultImageService = generateDefaultImageService;
            _filesystem = filesystem;
            _fileRepository = fileRepository;
            _service = service;
            _importParser = importParser;
        }

        public async Task<PaginatedList<EmployeeDto>> List(EmployeePageOptions options)
        {
            var paginatedList = await _employeeRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            var dtos = paginatedList.List.Select(e => EmployeeDto.Convert(e));

            return new PaginatedList<EmployeeDto>(dtos, paginatedList.TotalCount);
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return EmployeeDto.Convert(employee);
        }

        public async Task<ICollection<string>> GetEmploymentStatuses()
        {
            return await _employeeRepository.GetEmploymentStatuses();
        }

        public async Task<string> GetImagePathById(int id)
        {
            return await _employeeRepository.GetImagePathByIdAsync(id);
        }

        public async Task<EmployeeDto> Create(CreateEmployeeDto dto)
        {
            var employee = new Employee()
            {
                OrganizationID = _currentUser.OrganizationId,
                CreatedBy = _currentUser.DesktopUserId,
            };

            Map(dto, employee);
            await _employeeRepository.SaveAsync(employee);

            employee.OriginalImageId = (await CreateOriginalImage(employee)).Id;
            await _employeeRepository.SaveAsync(employee);

            return EmployeeDto.Convert(employee);
        }

        public async Task<EmployeeDto> Update(int id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            employee.LastUpdBy = _currentUser.DesktopUserId;
            Map(dto, employee);

            await _employeeRepository.SaveAsync(employee);

            return EmployeeDto.Convert(employee);
        }

        private void Map(CrudEmployeeDto dto, Employee employee)
        {
            employee.EmployeeNo = dto.EmployeeNo;
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.BirthDate = dto.Birthdate;

            employee.HomeAddress = dto.Address;
            employee.HomePhone = dto.LandlineNo;
            employee.MobilePhone = dto.MobileNo;
            employee.EmailAddress = dto.EmailAddress;

            employee.TinNo = dto.Tin;
            employee.SssNo = dto.SssNo;
            employee.PhilHealthNo = dto.PhilHealthNo;
            employee.HdmfNo = dto.PagIbigNo;

            employee.EmploymentStatus = dto.EmploymentStatus;
            employee.StartDate = dto.StartDate;
            employee.DateRegularized = dto.RegularizationDate;
            employee.EmploymentPolicyId = dto.EmploymentPolicyId;

            employee.PositionID = dto.PositionId;
        }

        private async Task<Data.Entities.File> CreateOriginalImage(Employee employee)
        {
            using var virtualFile = _generateDefaultImageService.Create(employee);
            var path = $"Employee/{employee.RowID.Value}/{virtualFile.Filename}";

            await _filesystem.Move(virtualFile.Stream, path);

            var file = new Data.Entities.File(
                key: virtualFile.Filename,
                path: path,
                filename: virtualFile.Filename,
                mediaType: "image/jpeg",
                size: virtualFile.Size);

            //file.CreatedById = _currentUser.UserId;
            //file.UpdatedById = file.CreatedById;

            await _fileRepository.Create(file);

            return file;
        }

        public async Task GenerateEmployeesImages()
        {
            var employees = (await _employeeRepository.GetEmployeesWithoutImageAsync()).ToList();

            foreach (var employee in employees)
            {
                employee.OriginalImage = await CreateOriginalImage(employee);
            }

            await _employeeRepository.SaveManyAsync(employees);
        }

        internal async Task<EmployeeImportParser.EmployeeImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.DesktopUserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            var employees = await _service.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, userId: userId);

            foreach (var employee in employees)
            {
                employee.OriginalImage = await CreateOriginalImage(employee);
            }
            if (employees.Any()) await _employeeRepository.SaveManyAsync(employees);

            return parsedResult;
        }
    }
}
