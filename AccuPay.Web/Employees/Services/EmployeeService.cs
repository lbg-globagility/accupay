using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.Employees;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly GenerateDefaultImageService _generateDefaultImageService;
        private readonly IFilesystem _filesystem;
        private readonly IFileRepository _fileRepository;
        private readonly IEmployeeDataService _dataService;
        private readonly IEmployeeImportParser _importParser;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ICurrentUser currentUser,
            GenerateDefaultImageService generateDefaultImageService,
            IFilesystem filesystem,
            IFileRepository fileRepository,
            IEmployeeDataService dataService,
            IEmployeeImportParser importParser)
        {
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _generateDefaultImageService = generateDefaultImageService;
            _filesystem = filesystem;
            _fileRepository = fileRepository;
            _dataService = dataService;
            _importParser = importParser;
        }

        public async Task<PaginatedList<EmployeeDto>> List(EmployeePageOptions options)
        {
            var employees = await _employeeRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            return employees.Select(e => EmployeeDto.Convert(e));
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
            var employee = Employee.NewEmployee(_currentUser.OrganizationId);

            Map(dto, employee);
            await _dataService.SaveAsync(employee, _currentUser.UserId);

            employee.OriginalImageId = (await CreateOriginalImage(employee)).Id;
            await _dataService.SaveAsync(employee, _currentUser.UserId);

            return EmployeeDto.Convert(employee);
        }

        public async Task<EmployeeDto> Update(int id, UpdateEmployeeDto dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            Map(dto, employee);

            await _dataService.SaveAsync(employee, _currentUser.UserId);

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

        private async Task<AccuPay.Core.Entities.File> CreateOriginalImage(Employee employee)
        {
            using var virtualFile = _generateDefaultImageService.Create(employee);
            var path = $"Employee/{employee.RowID.Value}/{virtualFile.Filename}";

            await _filesystem.Move(virtualFile.Stream, path);

            var file = new AccuPay.Core.Entities.File(
                key: virtualFile.Filename,
                path: path,
                filename: virtualFile.Filename,
                mediaType: "image/jpeg",
                size: virtualFile.Size);

            file.CreatedById = _currentUser.UserId;
            file.UpdatedById = file.CreatedById;

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

            await _dataService.SaveManyAsync(employees, _currentUser.UserId);
        }

        internal async Task<EmployeeImportParser.EmployeeImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            var parsedEmployees = parsedResult.ValidRecords;
            var jobNames = parsedEmployees
                .Where(e => e.JobNotYetExists)
                .GroupBy(e => e.JobPosition)
                .Select(e => e.FirstOrDefault().JobPosition)
                .ToList();

            var employees = await _dataService.BatchApply(parsedResult.ValidRecords, jobNames, _currentUser.OrganizationId, userId);

            foreach (var employee in employees)
            {
                employee.OriginalImage = await CreateOriginalImage(employee);
            }
            if (employees.Any()) await _dataService.SaveManyAsync(employees, _currentUser.UserId);

            return parsedResult;
        }

        public async Task<PaginatedList<EmployeeDto>> GetUnregisteredEmployeeAsync(PageOptions options, string searchTerm)
        {
            var organizationId = _currentUser.OrganizationId;
            var clientId = _currentUser.ClientId;

            var employees = await _employeeRepository.GetUnregisteredEmployeeAsync(options, searchTerm, clientId, organizationId);

            return employees.Select(e => EmployeeDto.Convert(e));
        }
    }
}
