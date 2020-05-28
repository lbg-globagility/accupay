using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Employees.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Employees.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        internal async Task<Employee> Update(int id, EmployeeDto dto)
        {
            var employee = await GetEmployeeById(id);

            // this should intercepted, base on Globagility's Client ID
            // and the current User who made the http request
            employee.LastUpdBy = 1;

            ApplyUpdate(dto, employee);

            await Save(employee);

            return employee;
        }

        internal async Task<Employee> Create(EmployeeDto dto)
        {
            var employee = new Employee();

            // this should intercepted, base on Globagility's Client ID
            // and the current User who made the http request
            employee.OrganizationID = 2;
            employee.CreatedBy = 1;

            ApplyUpdate(dto, employee);

            await Save(employee);

            return employee;
        }

        private async Task Save(Employee employee)
        {
            List<Employee> employees = new List<Employee>() { employee };
            await _employeeRepository.SaveManyAsync(employees);
        }

        private static void ApplyUpdate(EmployeeDto dto, Employee employee)
        {
            employee.PositionID = dto.PositionID;
            employee.PayFrequencyID = dto.PayFrequencyID;
            employee.Salutation = dto.Salutation;
            employee.FirstName = dto.FirstName;
            employee.MiddleName = dto.MiddleName;
            employee.LastName = dto.LastName;
            employee.Surname = dto.Surname;
            employee.EmployeeNo = dto.EmployeeNo;
            employee.TinNo = dto.TinNo;
            employee.SssNo = dto.SssNo;
            employee.HdmfNo = dto.HdmfNo;
            employee.PhilHealthNo = dto.PhilHealthNo;
            employee.EmploymentStatus = dto.EmploymentStatus;
            employee.EmailAddress = dto.EmailAddress;
            employee.WorkPhone = dto.WorkPhone;
            employee.HomePhone = dto.HomePhone;
            employee.MobilePhone = dto.MobilePhone;
            employee.HomeAddress = dto.HomeAddress;
            employee.Nickname = dto.Nickname;
            employee.JobTitle = dto.JobTitle;
            employee.Gender = dto.Gender;
            employee.EmployeeType = dto.EmployeeType;
            employee.MaritalStatus = dto.MaritalStatus;
            employee.BirthDate = dto.BirthDate;
            employee.StartDate = dto.StartDate;
            employee.TerminationDate = dto.TerminationDate;
            employee.NoOfDependents = dto.NoOfDependents;
            employee.UndertimeOverride = dto.UndertimeOverride;
            employee.OvertimeOverride = dto.OvertimeOverride;
            employee.NewEmployeeFlag = dto.NewEmployeeFlag;
            employee.LeaveBalance = dto.LeaveBalance;
            employee.SickLeaveBalance = dto.SickLeaveBalance;
            employee.MaternityLeaveBalance = dto.MaternityLeaveBalance;
            employee.OtherLeaveBalance = dto.OtherLeaveBalance;
            employee.VacationLeaveAllowance = dto.VacationLeaveAllowance;
            employee.SickLeaveAllowance = dto.SickLeaveAllowance;
            employee.MaternityLeaveAllowance = dto.MaternityLeaveAllowance;
            employee.OtherLeaveAllowance = dto.OtherLeaveAllowance;
            employee.LeavePerPayPeriod = dto.LeavePerPayPeriod;
            employee.SickLeavePerPayPeriod = dto.SickLeavePerPayPeriod;
            employee.MaternityLeavePerPayPeriod = dto.MaternityLeavePerPayPeriod;
            employee.OtherLeavePerPayPeriod = dto.OtherLeavePerPayPeriod;
            employee.AlphalistExempted = dto.AlphalistExempted;
            employee.WorkDaysPerYear = dto.WorkDaysPerYear;
            employee.DayOfRest = dto.DayOfRest;
            employee.AtmNo = dto.AtmNo;
            employee.BankName = dto.BankName;
            employee.CalcHoliday = dto.CalcHoliday;
            employee.CalcSpecialHoliday = dto.CalcSpecialHoliday;
            employee.CalcNightDiff = dto.CalcNightDiff;
            employee.CalcNightDiffOT = dto.CalcNightDiffOT;
            employee.CalcRestDay = dto.CalcRestDay;
            employee.CalcRestDayOT = dto.CalcRestDayOT;
            employee.DateRegularized = dto.DateRegularized;
            employee.DateEvaluated = dto.DateEvaluated;
            employee.RevealInPayroll = dto.RevealInPayroll;
            employee.LateGracePeriod = dto.LateGracePeriod;
            employee.OffsetBalance = dto.OffsetBalance;
            employee.AgencyID = dto.AgencyID;
            employee.AdvancementPoints = dto.AdvancementPoints;
            employee.BPIInsurance = dto.BPIInsurance;
        }

        private async Task<Employee> GetEmployeeById(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        internal async Task<EmployeeDto> GeyByIdAsync(int id)
        {
            var employee = await GetEmployeeById(id);
            var dto = EmployeeDto.Convert(employee);

            return dto;
        }

        internal async Task<IEnumerable<Employee>> GetAllAsync(int organizationId)
        {
            return await _employeeRepository.GetAllAsync(organizationId);
        }
    }
}
