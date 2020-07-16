using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services.Imports.Employees
{
    public class EmployeeImportModel : EmployeeModel
    {
        private string[] FIXED_AND_MONTHLY_TYPES = { "monthly", "fixed" };

        private bool _noEmployeeNo;
        private bool _noEmployeeType;
        private bool _monthlyHasNoWorkDaysPerYear;
        private bool _noLastName;
        private bool _noFirstName;
        private bool _noBirthDate;
        private bool _invalidBirthDate;
        private bool _noGender;
        private bool _noMaritalStatus;
        private bool _noJob;
        private bool _noEmploymentDate;
        private bool _invalidEmploymentDate;
        private bool _noEmploymentStatus;
        private bool _employeeAlreadyExists;

        private readonly Employee _employee;
        private readonly EmployeeRowRecord _parsedEmployee;

        public EmployeeImportModel(Employee employee, EmployeeRowRecord parsedEmployee)
        {
            _employeeAlreadyExists = employee != null;

            _employee = employee;
            _parsedEmployee = parsedEmployee;

            ApplyData(_employee, _parsedEmployee);
        }

        public string Remarks { get; internal set; }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployeeNo
                    || _employeeAlreadyExists
                    || _monthlyHasNoWorkDaysPerYear
                    || _noLastName
                    || _noFirstName
                    || _noBirthDate
                    || _invalidBirthDate
                    || _noGender
                    || _noMaritalStatus
                    || _noJob
                    || _noEmploymentDate
                    || _invalidEmploymentDate
                    || _noEmploymentStatus;
            }
        }

        public bool IsExistingEmployee { get; internal set; }

        private void ApplyData(Employee _employee, EmployeeRowRecord _parsedEmployee)
        {
            _noEmployeeNo = string.IsNullOrWhiteSpace(_parsedEmployee.EmployeeNo);
            _noEmployeeType = string.IsNullOrWhiteSpace(_parsedEmployee.EmployeeType);

            string validEmployeeType(string x) => _noEmployeeType ? x : _parsedEmployee.EmployeeType.ToLower();
            _monthlyHasNoWorkDaysPerYear = FIXED_AND_MONTHLY_TYPES.Any(t => t == validEmployeeType(t)) && _parsedEmployee.WorkDaysPerYear == 0;

            _noLastName = string.IsNullOrWhiteSpace(_parsedEmployee.LastName);
            _noFirstName = string.IsNullOrWhiteSpace(_parsedEmployee.FirstName);

            _noBirthDate = !_parsedEmployee.Birthdate.HasValue;
            _invalidBirthDate = !_noBirthDate && _parsedEmployee.Birthdate.Value < PayrollTools.SqlServerMinimumDate;

            _noGender = string.IsNullOrWhiteSpace(_parsedEmployee.Gender);
            _noMaritalStatus = string.IsNullOrWhiteSpace(_parsedEmployee.MaritalStatus);
            //_noJob = string.IsNullOrWhiteSpace(JobPosition);
            _noEmploymentDate = !_parsedEmployee.DateEmployed.HasValue;
            _invalidEmploymentDate = !_noEmploymentDate && _parsedEmployee.DateEmployed.Value < PayrollTools.SqlServerMinimumDate;
            _noEmploymentStatus = string.IsNullOrWhiteSpace(_parsedEmployee.EmploymentStatus);

            if (_employeeAlreadyExists)
            {
                if (!_noFirstName) _employee.FirstName = _parsedEmployee.FirstName;
                _employee.MiddleName = _parsedEmployee.MiddleName;
                if (!_noLastName) _employee.LastName = _parsedEmployee.LastName;
                _employee.TinNo = _parsedEmployee.Tin;
                _employee.SssNo = _parsedEmployee.SssNo;
                _employee.HdmfNo = _parsedEmployee.PagIbigNo;
                _employee.PhilHealthNo = _parsedEmployee.PhilHealthNo;
                if (!_noEmploymentStatus) _employee.EmploymentStatus = _parsedEmployee.EmploymentStatus;
                _employee.MobilePhone = _parsedEmployee.ContactNo;
                _employee.HomeAddress = _parsedEmployee.Address;
                if (!_noGender) _employee.Gender = _parsedEmployee.Gender;
                if (!_noEmployeeType) _employee.EmployeeType = _parsedEmployee.EmployeeType;
                if (!_noMaritalStatus) _employee.MaritalStatus = _parsedEmployee.MaritalStatus;
                if (!_noBirthDate) _employee.BirthDate = _parsedEmployee.Birthdate.Value;
                if (!_noEmploymentDate) _employee.StartDate = _parsedEmployee.DateEmployed.Value;
                _employee.AtmNo = _parsedEmployee.AtmAccountNo;
                if (!_monthlyHasNoWorkDaysPerYear) _employee.WorkDaysPerYear = _parsedEmployee.WorkDaysPerYear;
                return;
            }

            EmployeeNo = !_noEmployeeNo ? _parsedEmployee.EmployeeNo : string.Empty;
            //FullName = _parsedEmployee.;
            //PayFrequencyID = _parsedEmployee.;
            FirstName = _parsedEmployee.FirstName;
            MiddleName = _parsedEmployee.MiddleName;
            LastName = !_noLastName ? _parsedEmployee.LastName : string.Empty;
            Tin = _parsedEmployee.Tin;
            SssNo = _parsedEmployee.SssNo;
            PagIbigNo = _parsedEmployee.PagIbigNo;
            PhilHealthNo = _parsedEmployee.PhilHealthNo;
            EmploymentStatus = !_noEmploymentStatus ? _parsedEmployee.EmploymentStatus : string.Empty;
            //EmailAddress = _parsedEmployee.;
            //WorkPhone = _parsedEmployee.WorkPhone;
            //LandlineNo = _parsedEmployee.;
            MobileNo = _parsedEmployee.ContactNo;
            Address = _parsedEmployee.Address;
            Gender = !_noGender ? _parsedEmployee.Gender : string.Empty;
            EmployeeType = !_noEmployeeType ? _parsedEmployee.EmployeeType : string.Empty;
            MaritalStatus = !_noMaritalStatus ? _parsedEmployee.MaritalStatus : string.Empty;
            if (!_noBirthDate) Birthdate = _parsedEmployee.Birthdate;
            StartDate = _parsedEmployee.DateEmployed;
            //TerminationDate = _parsedEmployee.;
            //NoOfDependents = _parsedEmployee.;
            //NewEmployeeFlag = _parsedEmployee.;
            //AlphalistExempted = _parsedEmployee.;
            //DayOfRest = _parsedEmployee.;
            AtmNo = _parsedEmployee.AtmAccountNo;
            //BankName = _parsedEmployee.;
            //DateRegularized = _parsedEmployee.;
            //DateEvaluated = _parsedEmployee.;
            //RevealInPayroll = _parsedEmployee.;
            //AgencyID = _parsedEmployee.;
            //AdvancementPoints = _parsedEmployee.;
            //BPIInsurance = _parsedEmployee.;
            if (!_monthlyHasNoWorkDaysPerYear) WorkDaysPerYear = _parsedEmployee.WorkDaysPerYear;
            JobPosition = _parsedEmployee.JobPosition;

            //if (employee.EmploymentPolicy != null)
            //{
            //    EmploymentPolicy = new EmploymentPolicyDto()
            //    {
            //        Id = employee.EmploymentPolicy.Id,
            //        Name = employee.EmploymentPolicy.Name,
            //    };
            //}

            //if (employee.Position != null)
            //{
            //    Position = new PositionDto()
            //    {
            //        Id = employee.Position.RowID.Value,
            //        Name = employee.Position.Name
            //    };
            //}
        }

        internal void DescribeErrors()
        {
            List<string> errors = new List<string>();

            if (_noEmployeeNo) errors.Add("no Employee ID");
            if (_employeeAlreadyExists) errors.Add("employee already exists");
            if (_noLastName) errors.Add("no Last Name");
            if (_noFirstName) errors.Add("no First Name");
            if (_noBirthDate) errors.Add("no Birth Date");
            if (_invalidBirthDate) errors.Add("Birth Date cannot be earlier than January 1, 1753");
            if (_noGender) errors.Add("no Gender");
            if (_noMaritalStatus) errors.Add("no Marital Status");
            if (_noJob) errors.Add("no Job Position");
            if (_noEmploymentDate) errors.Add("no Employment Date");
            if (_invalidEmploymentDate) errors.Add("Employment Date cannot be earlier than January 1, 1753");
            if (_noEmploymentStatus) errors.Add("no Employment Status");
            if (_monthlyHasNoWorkDaysPerYear) errors.Add("no Work Days Per Year");

            Remarks = string.Join("; ", errors.ToArray());
        }

        public Employee Employee
        {
            get
            {
                if (_employeeAlreadyExists) return _employee;

                var employee = new Employee()
                {
                    HomeAddress = Address,
                    AtmNo = AtmNo,
                    //Branch= _parsedEmployee.Branch,
                    MobilePhone = MobileNo,
                    SickLeaveBalance = _parsedEmployee.CurrentSickLeaveBalance.HasValue ? _parsedEmployee.CurrentSickLeaveBalance.Value : 0,
                    LeaveBalance = _parsedEmployee.CurrentVacationLeaveBalance.HasValue ? _parsedEmployee.CurrentVacationLeaveBalance.Value : 0,
                    EmployeeNo = EmployeeNo,
                    EmployeeType = EmployeeType,
                    EmploymentStatus = EmploymentStatus,
                    FirstName = FirstName,
                    Gender = Gender,
                    //Position= _parsedEmployee.JobPosition,
                    LastName = LastName,
                    //= _parsedEmployee.LineNumber,
                    MaritalStatus = MaritalStatus,
                    MiddleName = MiddleName,
                    Nickname = _parsedEmployee.Nickname,
                    HdmfNo = PagIbigNo,
                    PhilHealthNo = PhilHealthNo,
                    Salutation = _parsedEmployee.Salutation,
                    SickLeaveAllowance = _parsedEmployee.SickLeaveAllowanceAnnual.HasValue ? _parsedEmployee.SickLeaveAllowanceAnnual.Value : 0,
                    SssNo = SssNo,
                    TinNo = Tin,
                    VacationLeaveAllowance = _parsedEmployee.VacationLeaveAllowanceAnnual.HasValue ? _parsedEmployee.SickLeaveAllowanceAnnual.Value : 0,
                    WorkDaysPerYear = WorkDaysPerYear
                };

                if (Birthdate.HasValue) employee.BirthDate = Birthdate.Value;
                if (StartDate.HasValue) employee.StartDate = StartDate.Value;

                return employee;
            }
        }
    }
}