using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Salaries
{
    public class SalaryImportModel : SalaryModel
    {
        private readonly Employee _employee;
        private readonly SalaryRowRecord _parsedSalary;
        private readonly bool _isOverlappedSalary;
        private readonly Salary _overlappedSalary;
        private readonly bool _noEmployee;
        private bool _noEmployeeNo;
        private bool _noAllowanceSalary;
        private bool _noBasicSalary;
        private bool _noEffectiveFrom;

        public SalaryImportModel(Employee employee, Salary overlappedSalary, SalaryRowRecord parsedSalary)
        {
            _noEmployee = employee == null;
            _employee = employee;

            _parsedSalary = parsedSalary;

            _isOverlappedSalary = overlappedSalary != null;
            _overlappedSalary = overlappedSalary;

            ApplyData(employee, parsedSalary);
        }

        private void ApplyData(Employee employee, SalaryRowRecord parsedSalary)
        {
            if (!_noEmployee)
            {
                EmployeeId = employee.RowID.Value;
                EmployeeName = employee.FullNameLastNameFirst;
            }

            _noEmployeeNo = string.IsNullOrWhiteSpace(parsedSalary.EmployeeNo);
            if (!_noEmployeeNo) EmployeeNo = parsedSalary.EmployeeNo;

            _noAllowanceSalary = !parsedSalary.AllowanceSalary.HasValue;
            if (!_noAllowanceSalary) AllowanceSalary = parsedSalary.AllowanceSalary.Value;

            _noBasicSalary = !parsedSalary.BasicSalary.HasValue;
            if (!_noBasicSalary) BasicSalary = parsedSalary.BasicSalary.Value;

            _noEffectiveFrom = !parsedSalary.EffectiveFrom.HasValue;
            if (!_noEffectiveFrom) EffectiveFrom = parsedSalary.EffectiveFrom.Value;
        }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployee ||
                    _noEmployeeNo ||
                    _isOverlappedSalary ||
                    _noAllowanceSalary ||
                    _noBasicSalary ||
                    _noEffectiveFrom;
            }
        }

        public bool HasOverlappedSalary
        {
            get
            {
                return _isOverlappedSalary;
            }
        }

        public int? EmployeeId { get; internal set; }

        internal void DescribeErrors()
        {
            List<string> errors = new List<string>();

            if (_noEmployee) errors.Add("Employee doesn't exists");
            if (_noEmployeeNo) errors.Add("Invalid Employee No");
            if (_isOverlappedSalary) errors.Add("Overlapped other salary");
            if (_noAllowanceSalary) errors.Add("Invalid Allowance Salary");
            if (_noBasicSalary) errors.Add("Invalid Basic Salary");
            if (_noEffectiveFrom) errors.Add("Invalid Effective From");

            Remarks = string.Join("; ", errors.ToArray());
        }
    }
}