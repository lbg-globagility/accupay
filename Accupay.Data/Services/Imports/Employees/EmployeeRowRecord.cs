using AccuPay.Data.Interfaces.Excel;
using AccuPay.Utilities.Attributes;
using System;

namespace AccuPay.Data.Services.Imports.Employees
{
    public class EmployeeRowRecord : IExcelRowRecord
    {
        public int LineNumber { get; set; }

        [ColumnName("Employee ID")]
        public string EmployeeNo { get; set; }

        [ColumnName("Last name")]
        public string LastName { get; set; }

        [ColumnName("First name")]
        public string FirstName { get; set; }

        [ColumnName("Middle name")]
        public string MiddleName { get; set; }

        [ColumnName("Birth date(MM/dd/yyyy)")]
        public DateTime? Birthdate { get; set; }

        [ColumnName("Gender(M/F)")]
        public string Gender { get; set; }

        [ColumnName("Nickname")]
        public string Nickname { get; set; }

        [ColumnName("Marital Status(Single/Married/N/A)")]
        public string MaritalStatus { get; set; }

        [ColumnName("Salutation")]
        public string Salutation { get; set; }

        [ColumnName("Address")]
        public string Address { get; set; }

        [ColumnName("Contact No.")]
        public string ContactNo { get; set; }

        [ColumnName("Job Position")]
        public string JobPosition { get; set; }

        [ColumnName("TIN")]
        public string Tin { get; set; }

        [ColumnName("SSS No.")]
        public string SssNo { get; set; }

        [ColumnName("PAGIBIG No.")]
        public string PagIbigNo { get; set; }

        [ColumnName("PhilHealth No.")]
        public string PhilHealthNo { get; set; }

        [ColumnName("Date employed(MM/dd/yyyy)")]
        public DateTime? DateEmployed { get; set; }

        [ColumnName("Employee Type(Daily/Monthly/Fixed)")]
        public string EmployeeType { get; set; }

        [ColumnName("Employment status(Probationary/Regular/Resigned/Terminated)")]
        public string EmploymentStatus { get; set; }

        [ColumnName("VL allowance per year (hours)")]
        public decimal? VacationLeaveAllowanceAnnual { get; set; }

        [ColumnName("SL allowance per year (hours)")]
        public decimal? SickLeaveAllowanceAnnual { get; set; }

        [ColumnName("Works days per year")]
        public decimal WorkDaysPerYear { get; set; }

        [ColumnName("Branch")]
        public string Branch { get; set; }

        [ColumnName("Current VL balance (hours)")]
        public decimal? CurrentVacationLeaveBalance { get; set; }

        [ColumnName("Current SL balance (hours)")]
        public decimal? CurrentSickLeaveBalance { get; set; }

        [ColumnName("ATM No./Account No.")]
        public string AtmAccountNo { get; set; }
    }
}