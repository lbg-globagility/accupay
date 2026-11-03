using System;

namespace AccuPay.Core.Services.Reports
{
    public class RemittanceReportRow
    {
        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string SssNumber { get; set; }

        public string PhilHealthNumber { get; set; }

        public string PagIbigNumber { get; set; }

        public string TinNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public decimal TotalBasic { get; set; }

        public decimal TotalCompensation { get; set; }

        public decimal SssPremium { get; set; }

        public decimal Ecc { get; set; }

        public decimal MandatoryProvidentFund { get; set; }

        /// <summary>
        /// Employee and employer SSS share. Includes the premium, ECC and MPF.
        /// </summary>
        public decimal TotalSss { get; set; }

        public decimal SssSalaryLoan { get; set; }

        public decimal SssCalamityLoan { get; set; }

        public decimal SssEmergencyLoan { get; set; }

        public decimal HdmfSalaryLoan { get; set; }

        public decimal HdmfCalamityLoan { get; set; }

        public decimal HdmfMp2 { get; set; }

        public decimal HdmfEmployeeShare { get; set; }

        public decimal HdmfEmployerShare { get; set; }

        /// <summary>
        /// Employee and employer HDMF share.
        /// </summary>
        public decimal TotalHdmf => HdmfEmployeeShare + HdmfEmployerShare;

        public decimal TotalTax { get; set; }

        public decimal PhilHealthEmployeeShare { get; set; }

        public decimal PhilHealthEmployerShare { get; set; }

        /// <summary>
        /// Employee and employer PhilHealth share.
        /// </summary>
        public decimal TotalPhilHealth => PhilHealthEmployeeShare + PhilHealthEmployerShare;
    }
}
