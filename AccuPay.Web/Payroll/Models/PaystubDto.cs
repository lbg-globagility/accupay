using AccuPay.Data.Services;
using AccuPay.Web.Employees.Models;

namespace AccuPay.Web.Payroll
{
    public class PaystubDto : BaseEmployeeDto
    {
        public static PaystubDto Convert(PaystubData paystubData)
        {
            var dto = new PaystubDto();
            if (paystubData?.Paystub?.Employee == null) return dto;

            dto.ApplyData(paystubData);

            return dto;
        }

        protected void ApplyData(PaystubData paystubData)
        {
            if (paystubData == null) return;

            var paystub = paystubData?.Paystub;
            var employee = paystubData?.Paystub?.Employee;

            if (paystub?.Employee == null) return;

            base.ApplyData(employee);
            if (paystub.EmployeeID != paystub?.Employee.RowID) return;

            BasicRate = paystubData.BasicRate;

            Id = paystub.RowID.Value;
            EmployeeId = paystub.EmployeeID.Value;
            PayperiodId = paystub.PayPeriodID.Value;
            BasicHours = paystub.BasicHours;
            BasicPay = paystub.BasicPay;
            RegularHours = paystub.RegularHours;
            RegularPay = paystub.RegularPay;
            OvertimeHours = paystub.OvertimeHours;
            OvertimePay = paystub.OvertimePay;
            NightDiffHours = paystub.NightDiffHours;
            NightDiffPay = paystub.NightDiffPay;
            NightDiffOvertimeHours = paystub.NightDiffOvertimeHours;
            NightDiffOvertimePay = paystub.NightDiffOvertimePay;
            RestDayHours = paystub.RestDayHours;
            RestDayPay = paystub.RestDayPay;
            RestDayOTHours = paystub.RestDayOTHours;
            RestDayOTPay = paystub.RestDayOTPay;
            LeaveHours = paystub.LeaveHours;
            LeavePay = paystub.LeavePay;
            SpecialHolidayHours = paystub.SpecialHolidayHours;
            SpecialHolidayPay = paystub.SpecialHolidayPay;
            SpecialHolidayOTHours = paystub.SpecialHolidayOTHours;
            SpecialHolidayOTPay = paystub.SpecialHolidayOTPay;
            RegularHolidayHours = paystub.RegularHolidayHours;
            RegularHolidayPay = paystub.RegularHolidayPay;
            RegularHolidayOTHours = paystub.RegularHolidayOTHours;
            RegularHolidayOTPay = paystub.RegularHolidayOTPay;
            LateHours = paystub.LateHours;
            LateDeduction = paystub.LateDeduction;
            UndertimeHours = paystub.UndertimeHours;
            UndertimeDeduction = paystub.UndertimeDeduction;
            AbsentHours = paystub.AbsentHours;
            AbsenceDeduction = paystub.AbsenceDeduction;
            GrossPay = paystub.GrossPay;
            TotalAdjustments = paystub.TotalAdjustments;
            TotalEarnings = paystub.TotalEarnings;
            TotalBonus = paystub.TotalBonus;
            TotalNonTaxableAllowance = paystub.TotalAllowance;
            TotalTaxableAllowance = paystub.TotalTaxableAllowance;
            TaxableIncome = paystub.TaxableIncome;
            WithholdingTax = paystub.WithholdingTax;
            SssEmployeeShare = paystub.SssEmployeeShare;
            PhilHealthEmployeeShare = paystub.PhilHealthEmployeeShare;
            HdmfEmployeeShare = paystub.HdmfEmployeeShare;
            TotalLoans = paystub.TotalLoans;
            TotalDeductions = paystub.NetDeductions;
            NetPay = paystub.NetPay;

            Salary = new SalaryDto()
            {
                Id = paystubData.Salary.RowID.Value,
                BasicAmount = paystubData.Salary.BasicSalary,
                AllowanceAmount = paystubData.Salary.AllowanceSalary,
                HourlyRate = paystubData.HourlyRate,
                DailyRate = paystubData.DailyRate,
                SalaryType = paystubData.Paystub.Employee.EmployeeType,
            };
        }

        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int PayperiodId { get; set; }

        public decimal BasicRate { get; set; }

        public decimal BasicHours { get; set; }

        public decimal BasicPay { get; set; }

        public decimal RegularHours { get; set; }

        public decimal RegularPay { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimePay { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffPay { get; set; }

        public decimal NightDiffOvertimeHours { get; set; }

        public decimal NightDiffOvertimePay { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayPay { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal RestDayOTPay { get; set; }

        public decimal LeaveHours { get; set; }

        public decimal LeavePay { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        public decimal LateHours { get; set; }

        public decimal LateDeduction { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal UndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal AbsenceDeduction { get; set; }

        public decimal GrossPay { get; set; }

        public decimal TotalAdjustments { get; set; }

        public decimal TotalEarnings { get; set; }

        public decimal TotalBonus { get; set; }

        public decimal TotalNonTaxableAllowance { get; set; }

        public decimal TotalTaxableAllowance { get; set; }

        public decimal TaxableIncome { get; set; }

        public decimal WithholdingTax { get; set; }

        public decimal SssEmployeeShare { get; set; }

        public decimal PhilHealthEmployeeShare { get; set; }

        public decimal HdmfEmployeeShare { get; set; }

        public decimal TotalLoans { get; set; }

        public decimal TotalDeductions { get; set; }

        public decimal NetPay { get; set; }

        public SalaryDto Salary { get; set; }

        public class SalaryDto
        {
            public int Id { get; set; }

            public decimal BasicAmount { get; set; }

            public decimal AllowanceAmount { get; set; }

            public string SalaryType { get; set; }

            public decimal DailyRate { get; set; }

            public decimal HourlyRate { get; set; }
        }
    }
}
