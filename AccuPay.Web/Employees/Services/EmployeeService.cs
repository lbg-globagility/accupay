using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Files.Services;
using System;
using System.Collections.Generic;
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

        public EmployeeService(EmployeeRepository employeeRepository,
            ICurrentUser currentUser,
            GenerateDefaultImageService generateDefaultImageService,
            IFilesystem filesystem,
            FileRepository fileRepository)
        {
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _generateDefaultImageService = generateDefaultImageService;
            _filesystem = filesystem;
            _fileRepository = fileRepository;
        }

        public async Task<PaginatedList<EmployeeDto>> PaginatedList(EmployeePageOptions options)
        {
            var paginatedList = await _employeeRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            var dtos = paginatedList.List.Select(e => EmployeeDto.Convert(e));

            return new PaginatedList<EmployeeDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            return EmployeeDto.Convert(employee);
        }

        private async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
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
                EmployeeNo = dto.EmployeeNo,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                BirthDate = dto.Birthdate,
                HomeAddress = dto.Address,
                HomePhone = dto.LandlineNo,
                MobilePhone = dto.MobileNo,
                EmailAddress = dto.EmailAddress,
                TinNo = dto.Tin,
                SssNo = dto.SssNo,
                PhilHealthNo = dto.PhilHealthNo,
                HdmfNo = dto.PagIbigNo
            };

            await Save(employee);

            employee.OriginalImageId = (await CreateOriginalImageIdAsync(employee)).Id;
            //employee.OriginalImage = await CreateOriginalImageIdAsync(employee);

            await _employeeRepository.Attach(employee);

            return EmployeeDto.Convert(employee);
        }

        public async Task<EmployeeDto> Update(int id, EmployeeDto dto)
        {
            var employee = await GetEmployeeByIdAsync(id);

            employee.LastUpdBy = _currentUser.DesktopUserId;

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

            await Save(employee);

            return EmployeeDto.Convert(employee);
        }

        private async Task Save(Employee employee)
        {
            await _employeeRepository.SaveAsync(employee);
        }

        public async Task<File> CreateOriginalImageIdAsync(Employee employee)
        {
            using var virtualFile = _generateDefaultImageService.Create(employee);
            var path = $"Employee/{employee.RowID.Value.ToString()}/{virtualFile.Filename}";

            await _filesystem.Move(virtualFile.Stream, path);

            var file = new File(
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
            var employees = await _employeeRepository.GetEmployeesWithoutImageAsync();

            foreach (var employee in employees)
            {
                employee.OriginalImage = await CreateOriginalImageIdAsync(employee);

                await _employeeRepository.SaveAsync(employee);
            }
        }
    }
}
