using AccuPay.Core.Entities;
using System;
using System.Linq;

namespace AccuPay.Core.Services.Reports.Employees_Personal_Information
{
    public class EmployeeRow
    {
        private readonly Employee _employee;

        public EmployeeRow(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException();

            _employee = employee;
        }

        public string EmployeeNumber => _employee.EmployeeNo;
        public string FirstName => _employee.FirstName;
        public string MiddleName => _employee.MiddleName;
        public string LastName => _employee.LastName;
        public string EmployeeType => _employee.EmployeeType;
        public string TerminationDate => _employee.TerminationDate?.ToShortDateString();
        public string EmploymentStatus => _employee.EmploymentStatus;
        public string StartDate => _employee.StartDate.ToShortDateString();
        public string Salutation => _employee.Salutation;
        public string Gender => _employee.Gender;
        public string MaritalStatus => _employee.MaritalStatus;

        public string DayOfRest
        {
            get
            {
                if (_employee.DayOfRest == null || !Enumerable.Range(0, 7).ToList().Contains(_employee.DayOfRest.Value - 1))
                    return "";

                return ((DayOfWeek)_employee.DayOfRest - 1).ToString();
            }
        }

        public string DateRegularized => _employee.DateRegularized?.ToShortDateString();
        public string DateEvaluated => _employee.DateEvaluated?.ToShortDateString();
        public string AtmNo => _employee.AtmNo;
        public string BankName => _employee.BankName;
        public string PositionName => _employee.Position?.Name;
        public string BirthDate => _employee.BirthDate.ToShortDateString();
        public string EmailAddress => _employee.EmailAddress;
        public string TinNo => _employee.TinNo;
        public string SssNo => _employee.SssNo;
        public string PhilHealthNo => _employee.PhilHealthNo;
        public string HdmfNo => _employee.HdmfNo;
        public string HomeAddress => _employee.HomeAddress;
        public string WorkPhone => _employee.WorkPhone;
        public string HomePhone => _employee.HomePhone;
        public string MobilePhone => _employee.MobilePhone;
        public decimal LateGracePeriod => _employee.LateGracePeriod;
        public decimal WorkDaysPerYear => _employee.WorkDaysPerYear;
        public string CalcHoliday => _employee.CalcHoliday ? "YES" : "NO";
        public string CalcSpecialHoliday => _employee.CalcSpecialHoliday ? "YES" : "NO";
        public string CalcNightDiff => _employee.CalcNightDiff ? "YES" : "NO";
        public string CalcRestDay => _employee.CalcRestDay ? "YES" : "NO";
        public decimal VacationLeaveAllowance => _employee.VacationLeaveAllowance;
        public decimal SickLeaveAllowance => _employee.SickLeaveAllowance;
        public decimal BPIInsurance => _employee.BPIInsurance;
        public string AgencyName => _employee.Agency?.Name;
        public string BranchName => _employee.Branch?.Name;
    }
}