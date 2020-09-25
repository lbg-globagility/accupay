using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Allowances
{
    public class AllowanceImportModel : AllowanceModel
    {
        private AllowanceRowRecord _parsedAllowance;
        private Employee _employee;
        private readonly Allowance _allowance;
        private AllowanceType _allowanceType;
        private readonly bool _noEmployee;
        private readonly bool _noAllowanceType;
        private bool _noAllowanceFrequency;
        private bool _noAllowanceName;
        private bool _noAmount;
        private bool _noEffectiveStartDate;
        private bool _noEmployeeNo;
        private bool _fixedAllowanceNotSetToMonthlyFrequency;
        private bool _hasDuplicate;

        public AllowanceImportModel(AllowanceRowRecord parsedAllowance, Employee employee, Allowance allowance, AllowanceType allowanceType)
        {
            _parsedAllowance = parsedAllowance;

            _noEmployee = employee == null;
            _employee = employee;

            _allowance = allowance;

            _noAllowanceType = allowanceType == null;
            _allowanceType = allowanceType;

            ApplyData(parsedAllowance, employee, allowanceType);
        }

        private void ApplyData(AllowanceRowRecord parsedAllowance, Employee employee, AllowanceType allowanceType)
        {
            if (!_noEmployee) EmployeeId = employee.RowID;

            if (!_noAllowanceType) AllowanceTypeId = allowanceType.Id;

            _noAllowanceFrequency = string.IsNullOrEmpty(parsedAllowance.AllowanceFrequency);
            if (!_noAllowanceFrequency) AllowanceFrequency = parsedAllowance.AllowanceFrequency;

            _noAllowanceName = string.IsNullOrEmpty(parsedAllowance.AllowanceName);
            if (!_noAllowanceName) AllowanceName = parsedAllowance.AllowanceName;

            _noAmount = !parsedAllowance.Amount.HasValue;
            if (!_noAmount) Amount = parsedAllowance.Amount;

            _noEffectiveStartDate = !parsedAllowance.EffectiveStartDate.HasValue;
            if (!_noEffectiveStartDate) EffectiveStartDate = parsedAllowance.EffectiveStartDate;

            _noEmployeeNo = string.IsNullOrEmpty(parsedAllowance.EmployeeNo);
            if (!_noEmployeeNo) EmployeeNo = parsedAllowance.EmployeeNo;

            // This already base on AllownceType not on Product.`Allowance Type`
            if (!IsAllowanceTypeNotYetExists && !_noAllowanceFrequency && !_noAllowanceName)
            {
                _fixedAllowanceNotSetToMonthlyFrequency = _allowanceType.IsFixed && AllowanceFrequency != Allowance.FREQUENCY_MONTHLY;
            }

            if (_allowance != null)
            {
                bool isEqualToLowerCase(string dataText, string parsedText) => string.IsNullOrWhiteSpace(parsedText) ? false : dataText.ToLower() == parsedText.ToLower();

                var sameEmployee = _noEmployee ? false : _allowance.EmployeeID == EmployeeId;
                var sameAllowanceType = _noAllowanceType ? false : isEqualToLowerCase(_allowance.Type, AllowanceName);
                var sameEffectiveStartDate = _noEffectiveStartDate ? false : _allowance.EffectiveStartDate == EffectiveStartDate.Value;
                var sameEffectiveEndDate = _allowance.EffectiveEndDate == EffectiveEndDate;
                var sameAmount = _noAmount ? false : _allowance.Amount == Amount.Value;

                _hasDuplicate = sameEmployee && sameAllowanceType && sameEffectiveStartDate && sameEffectiveEndDate && sameAmount;
            }
        }

        public bool IsAllowanceTypeNotYetExists
        {
            get
            {
                return _noAllowanceType && !_noAllowanceName;
            }
        }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployee ||
                    _noAllowanceFrequency ||
                    _noAllowanceName ||
                    _noAmount ||
                    _noEffectiveStartDate ||
                    _noEmployeeNo ||
                    _fixedAllowanceNotSetToMonthlyFrequency ||
                    _hasDuplicate;
            }
        }

        internal void DescribeError()
        {
            var errors = new List<string>();

            if (_noEmployee) errors.Add("Employee doesn't exists");
            if (_noAllowanceFrequency) errors.Add("Invalid Allowance frequency");
            if (_noAllowanceName) errors.Add("Invalid Name of allowance");
            if (_noAmount) errors.Add("Invalid Allowance amount");
            if (_noEffectiveStartDate) errors.Add("Invalid Effective start date");
            if (_noEmployeeNo) errors.Add("Invalid EmployeeID");
            if (_fixedAllowanceNotSetToMonthlyFrequency) errors.Add("Only fixed allowance type are allowed for Monthly allowances.");
            if (_hasDuplicate) errors.Add("Allowance already exists");

            Remarks = string.Join("; ", errors.ToArray());
        }

        public string Remarks { get; set; }
    }
}