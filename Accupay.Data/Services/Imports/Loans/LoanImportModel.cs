using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Data.Services.Imports.Loans
{
    public class LoanImportModel : LoanModel
    {
        private readonly LoanRowRecord _loan;
        private bool _noEmployee;
        private readonly Employee _employee;
        private bool _noLoanType;
        private readonly Product _loanType;
        private bool _noStartDate;
        private bool _noTotalLoanAmount;
        private bool _noTotalLoanBalance;
        private bool _noDeductionAmount;
        private bool _noDeductionSchedule;

        public LoanImportModel(LoanRowRecord loan, Employee employee, Product loanType)
        {
            _loan = loan;

            _noEmployee = employee == null;
            _employee = employee;

            _noLoanType = loanType == null;
            _loanType = loanType;

            ApplyData();
        }

        private void ApplyData()
        {
            EmployeeNo = _loan.EmployeeNo;
            if (!_noEmployee) EmployeeId = _employee.RowID.Value;
            LoanName = _loan.LoanName;
            if (!_noLoanType) LoanTypeId = _loanType.RowID.Value;
            LoanNumber = _loan.LoanNumber;

            _noStartDate = !_loan.StartDate.HasValue;
            if (!_noStartDate) StartDate = _loan.StartDate.Value;

            _noTotalLoanAmount = !_loan.TotalLoanAmount.HasValue;
            if (!_noTotalLoanAmount) TotalLoanAmount = _loan.TotalLoanAmount.Value;

            _noTotalLoanBalance = !_loan.TotalLoanBalance.HasValue;
            if (!_noTotalLoanBalance) TotalLoanBalance = _loan.TotalLoanBalance.Value;

            _noDeductionAmount = !_loan.DeductionAmount.HasValue;
            if (!_noDeductionAmount) DeductionAmount = _loan.DeductionAmount.Value;

            _noDeductionSchedule = string.IsNullOrEmpty(_loan.DeductionSchedule);
            if (!_noDeductionSchedule) DeductionSchedule = _loan.DeductionSchedule;

            Comments = _loan.Comments;
        }

        public bool EmployeeNotExists
        {
            get
            {
                return _noEmployee;
            }
        }

        public bool LoanTypeNotExists
        {
            get
            {
                return _noLoanType;
            }
        }

        public bool InvalidToSave
        {
            get
            {
                return _noEmployee ||
                    _noStartDate ||
                    _noTotalLoanAmount ||
                    _noTotalLoanBalance ||
                    _noDeductionAmount ||
                    _noDeductionSchedule;
            }
        }

        public string Remarks { get; internal set; }

        internal void DescribeError()
        {
            List<string> errors = new List<string>();

            if (_noEmployee) errors.Add("Employee not exists");
            if (_noStartDate) errors.Add("Start date is empty");
            if (_noTotalLoanAmount) errors.Add("Invalid Total loan amount");
            if (_noTotalLoanBalance) errors.Add("Invalid Loan balance");
            if (_noDeductionAmount) errors.Add("Invalid Deduction amount");
            if (_noDeductionSchedule) errors.Add("Invalid Deduction frequency");

            Remarks = string.Join("; ", errors.ToArray());
        }
    }
}