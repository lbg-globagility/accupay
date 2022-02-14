using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.ReportModels
{
    public class AlphalistModel
    {
        private const decimal BASIC_TAX_EXEMPTION = 0; //50000
        private const string SCHEDULE_71 = "Schedule 7.1";
        private const string SCHEDULE_73 = "Schedule 7.3";
        private const string SCHEDULE_74 = "Schedule 7.4";
        private const string SCHEDULE_75 = "Schedule 7.5";
        private readonly bool _actualSwitch;
        private readonly List<Paystub> _paystubs;

        public AlphalistModel(IGrouping<int?, Paystub> paystubs,
            DateTime startingDate,
            List<SalaryModel> latestSalaries,
            bool actualSwitch)
        {
            var employeeId = paystubs.Key;

            var employee = paystubs.
                FirstOrDefault(p => p.EmployeeID == employeeId).
                Employee;

            _startingDate = startingDate;

            var salary = latestSalaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);
            
            //SupplementaryBAmount = _paystub;
            EmployeeID = employee.EmployeeNo;
            FirstName = employee.FirstName;
            MiddleName = employee.MiddleName;
            LastName = employee.LastName;
            TinNo = employee.TinNo;
            //RdoCode = _employee.;
            StartDate = employee.StartDate;
            EndDate = employee.TerminationDate;
            Category = GetCategory(employee: employee, IsMinimumWageEarner: salary?.IsMinimumWage ?? false);
            EmploymentStatus = employee.EmploymentStatus;
            RegisteredAddress = employee.HomeAddress;
            //RegisteredZipCode = _paystub;
            LocalAddress = employee.HomeAddress;
            //LocalZipCode = _paystub;
            //ForeignAddress = _paystub;
            //ForeignZipCode = _paystub;
            Birthdate = employee.BirthDate;
            //TelephoneNo = _paystub;
            //ExemptionStatus = _paystub;
            //ExemptionCode = _paystub;
            //WifeClaim = _paystub;
            //DependentsName = _paystub;
            //DependentsBirthday = _paystub;
            var dummySalary = new Salary() { BasicSalary = salary?.BasicSalary ?? 0,
                AllowanceSalary = salary?.AllowanceSalary??0 };
            var monthlyRate = PayrollTools.GetEmployeeMonthlyRate(employee,
                dummySalary,
                actualSwitch);
            if(salary?.IsMinimumWage??false) MinimumWagePerDay = PayrollTools.GetDailyRate(monthlyRate,
                employee.WorkDaysPerYear);
            //MinimumWagePerMonth = _paystub;
            IsMinimumWageEarner = salary?.IsMinimumWage ?? false;
            WorkDaysPerYear = employee.WorkDaysPerYear;
            //PresentEmployerTinNo = _paystub;
            //PresentEmployerName = _paystub;
            //PresentEmployerAddress = _paystub;
            //PresentEmployerZipCode = _paystub;
            //PreviousEmployerTinNo = _paystub;
            //PreviousEmployerName = _paystub;
            //PreviousEmployerAddress = _paystub;
            //PreviousEmployerZipCode = _paystub;

            _actualSwitch = actualSwitch;
            _paystubs = paystubs.
                Where(p => p.EmployeeID == employeeId).
                ToList();

            //var totalEmpWithholdingTax = _paystubs.Sum(t => t.WithholdingTax);
            //Category = GetCategory(employee: employee, IsMinimumWageEarner: totalEmpWithholdingTax==0);

            GrossCompensationIncome = GetValue(PaystubProperty.GrossPay);
            TaxableIncome = GetValue(PaystubProperty.TaxableIncome);

            TotalExemptions = BASIC_TAX_EXEMPTION;
            TaxDue = GetValue(PaystubProperty.WithholdingTax);

            var basicSalary = GetValue(PaystubProperty.GrossPay) +
                GetValue(PaystubProperty.GrandTotalAllowance) +
                GetValue(PaystubProperty.TotalBonus);
            if(salary?.IsMinimumWage ?? false)
            {
                BasicSalary = basicSalary;
            }
            else
            {
                TaxableBasicSalary = basicSalary;
            }

            HolidayPay = GetValue(PaystubProperty.RegularHolidayPay) +
                GetValue(PaystubProperty.SpecialHolidayPay) +
                GetValue(PaystubProperty.RegularHolidayRestDayPay);
            OvertimePay = GetValue(PaystubProperty.OvertimePay) +
                GetValue(PaystubProperty.RegularHolidayOTPay) +
                GetValue(PaystubProperty.SpecialHolidayOTPay) +
                GetValue(PaystubProperty.RestDayOTPay);
            NightDiffPay = GetValue(PaystubProperty.NightDiffPay) +
                GetValue(PaystubProperty.RegularHolidayNightDiffPay) +
                GetValue(PaystubProperty.SpecialHolidayNightDiffPay) +
                GetValue(PaystubProperty.RestDayNightDiffPay);
            _13thMonthPay = GetValue(PaystubProperty.ThirteenMonthPay);
            GovernmentInsurance = GetValue(PaystubProperty.GovernmentDeductions);
            CostOfLivingAllowance = GetValue(PaystubProperty.Ecola);
        }

        private readonly DateTime _startingDate;

        public string EmployeeID { get; internal set; }
        public string FirstName { get; internal set; }
        public string MiddleName { get; internal set; }
        public string LastName { get; internal set; }
        public string TinNo { get; internal set; }
        public string RdoCode { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public DateTime? EndDate { get; internal set; }
        public string Category { get; internal set; }
        public string EmploymentStatus { get; internal set; }
        public string RegisteredAddress { get; internal set; }
        public string RegisteredZipCode { get; internal set; }
        public string LocalAddress { get; internal set; }
        public string LocalZipCode { get; internal set; }
        public string ForeignAddress { get; internal set; }
        public string ForeignZipCode { get; internal set; }
        public DateTime Birthdate { get; internal set; }
        public string TelephoneNo { get; internal set; }
        public string ExemptionStatus { get; internal set; }
        public string ExemptionCode { get; internal set; }
        public string WifeClaim { get; internal set; }
        public string DependentsName { get; internal set; }
        public string DependentsBirthday { get; internal set; }
        public decimal MinimumWagePerDay { get; internal set; }
        public string MinimumWagePerMonth { get; internal set; }
        public bool IsMinimumWageEarner { get; internal set; }
        public decimal WorkDaysPerYear { get; internal set; }
        public string PresentEmployerTinNo { get; internal set; }
        public string PresentEmployerName { get; internal set; }
        public string PresentEmployerAddress { get; internal set; }
        public string PresentEmployerZipCode { get; internal set; }
        public string PreviousEmployerTinNo { get; internal set; }
        public string PreviousEmployerName { get; internal set; }
        public string PreviousEmployerAddress { get; internal set; }
        public string PreviousEmployerZipCode { get; internal set; }
        public decimal GrossCompensationIncome { get; internal set; }
        public decimal NonTaxableIncome => BasicSalary + HolidayPay + OvertimePay + NightDiffPay + HazardPay + _13thMonthPay + DeMinimisBenefits + GovernmentInsurance + SalariesAndOtherCompensation;
        public decimal TaxableIncome { get; internal set; }
        public decimal PreviousTaxableIncome { get; internal set; }
        public decimal GrossTaxableIncome => TaxableIncome + PreviousTaxableIncome;
        public decimal TotalExemptions { get; internal set; }
        public decimal PremiumPaidOnHealth { get; internal set; }
        public decimal NetTaxableIncome {
            get
            {
                var difference = GrossTaxableIncome - TotalExemptions;
                return difference < 0 ? 0 : difference;
            }
        }
        public decimal TaxDue { get; internal set; }
        public decimal PresentTaxWithheld => TaxDue;
        public decimal PreviousTaxWithheld { get; internal set; }
        public decimal TotalTaxWithheld => PreviousTaxWithheld + PresentTaxWithheld;
        public decimal BasicSalary { get; internal set; }
        public decimal HolidayPay { get; internal set; }
        public decimal OvertimePay { get; internal set; }
        public decimal NightDiffPay { get; internal set; }
        public decimal HazardPay { get; internal set; }
        public decimal _13thMonthPay { get; internal set; }
        public decimal DeMinimisBenefits { get; internal set; }
        public decimal GovernmentInsurance { get; internal set; }
        public decimal SalariesAndOtherCompensation { get; internal set; }
        public decimal TotalNonTaxableIncome => BasicSalary +
            HolidayPay +
            OvertimePay +
            NightDiffPay +
            HazardPay +
            _13thMonthPay +
            DeMinimisBenefits +
            GovernmentInsurance +
            SalariesAndOtherCompensation;
        public decimal TaxableBasicSalary { get; internal set; }
        public decimal Representation { get; internal set; }
        public decimal Transportation { get; internal set; }
        public decimal CostOfLivingAllowance { get; internal set; }
        public decimal FixedHousingAllowance { get; internal set; }
        public decimal OthersAName { get; internal set; }
        public decimal OthersAAmount { get; internal set; }
        public decimal OthersBName { get; internal set; }
        public decimal OthersBAmount { get; internal set; }
        public decimal Commission { get; internal set; }
        public decimal ProfitSharing { get; internal set; }
        public decimal FeesInclDirectorsFees { get; internal set; }
        public decimal Taxable13thMonthPay { get; internal set; }
        public decimal TaxableHazardPay { get; internal set; }
        public decimal TaxableOvertimePay { get; internal set; }
        public string SupplementaryAName { get; internal set; }
        public decimal SupplementaryAAmount { get; internal set; }
        public decimal SupplementaryBName { get; internal set; }
        public decimal SupplementaryBAmount { get; internal set; }
        public decimal TotalTaxableIncome => TaxableBasicSalary +
            Representation +
            Transportation +
            CostOfLivingAllowance +
            FixedHousingAllowance +
            OthersAAmount +
            OthersBAmount +
            Commission +
            ProfitSharing +
            FeesInclDirectorsFees +
            Taxable13thMonthPay +
            TaxableHazardPay +
            TaxableOvertimePay +
            SupplementaryAAmount +
            SupplementaryBAmount;
        public string CtcNo { get; internal set; }
        public string CtcPlace { get; internal set; }
        public string CtcDate { get; internal set; }
        public string FullNameLastNameFirst => $"{LastName}, {FirstName}".Trim();

        private decimal GetValue(PaystubProperty p)
        {
            switch (p)
            {
                case PaystubProperty.TotalEarnings:
                    return _paystubs.Sum(t => t.TotalEarnings);
                case PaystubProperty.TotalBonus:
                    return _paystubs.Sum(t => t.TotalBonus);
                case PaystubProperty.TotalNonTaxableAllowance:
                    return _paystubs.Sum(t => t.TotalNonTaxableAllowance);
                case PaystubProperty.TotalTaxableAllowance:
                    return _paystubs.Sum(t => t.TotalTaxableAllowance);
                case PaystubProperty.DeferredTaxableIncome:
                    return _paystubs.Sum(t => t.DeferredTaxableIncome);
                case PaystubProperty.TaxableIncome:
                    return _paystubs.Sum(t => t.TaxableIncome);
                case PaystubProperty.WithholdingTax:
                    return _paystubs.Sum(t => t.WithholdingTax);
                case PaystubProperty.SssEmployeeShare:
                    return _paystubs.Sum(t => t.SssEmployeeShare);
                case PaystubProperty.SssEmployerShare:
                    return _paystubs.Sum(t => t.SssEmployerShare);
                case PaystubProperty.PhilHealthEmployeeShare:
                    return _paystubs.Sum(t => t.PhilHealthEmployeeShare);
                case PaystubProperty.PhilHealthEmployerShare:
                    return _paystubs.Sum(t => t.PhilHealthEmployerShare);
                case PaystubProperty.HdmfEmployeeShare:
                    return _paystubs.Sum(t => t.HdmfEmployeeShare);
                case PaystubProperty.HdmfEmployerShare:
                    return _paystubs.Sum(t => t.HdmfEmployerShare);
                case PaystubProperty.TotalLoans:
                    return _paystubs.Sum(t => t.TotalLoans);
                case PaystubProperty.Ecola:
                    return _paystubs.Sum(t => t.Ecola);
                case PaystubProperty.GrandTotalAllowance:
                    return _paystubs.Sum(t => t.GrandTotalAllowance);
                case PaystubProperty.NetDeductions:
                    return _paystubs.Sum(t => t.NetDeductions);
                case PaystubProperty.GovernmentDeductions:
                    return _paystubs.Sum(t => t.GovernmentDeductions);
                case PaystubProperty.TotalRestDayPay:
                    return _paystubs.Sum(t => t.TotalRestDayPay);
                case PaystubProperty.RegularPayAndTotalRestDay:
                    return _paystubs.Sum(t => t.RegularPayAndTotalRestDay);
                case PaystubProperty.TotalDeductionAdjustments:
                    return _paystubs.Sum(t => t.TotalDeductionAdjustments);
                case PaystubProperty.TotalAdditionAdjustments:
                    return _paystubs.Sum(t => t.TotalAdditionAdjustments);
                case PaystubProperty.RegularPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularPay) : _paystubs.Sum(t => t.RegularPay);
                case PaystubProperty.LateDeduction:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.LateDeduction) : _paystubs.Sum(t => t.LateDeduction);
                case PaystubProperty.UndertimeDeduction:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.UndertimeDeduction) : _paystubs.Sum(t => t.UndertimeDeduction);
                case PaystubProperty.AbsenceDeduction:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.AbsenceDeduction) : _paystubs.Sum(t => t.AbsenceDeduction);
                case PaystubProperty.OvertimePay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.OvertimePay) : _paystubs.Sum(t => t.OvertimePay);
                case PaystubProperty.NightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.NightDiffPay) : _paystubs.Sum(t => t.NightDiffPay);
                case PaystubProperty.NightDiffOvertimePay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.NightDiffOvertimePay) : _paystubs.Sum(t => t.NightDiffOvertimePay);
                case PaystubProperty.RestDayPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RestDayPay) : _paystubs.Sum(t => t.RestDayPay);
                case PaystubProperty.RestDayOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RestDayOTPay) : _paystubs.Sum(t => t.RestDayOTPay);
                case PaystubProperty.LeavePay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.LeavePay) : _paystubs.Sum(t => t.LeavePay);
                case PaystubProperty.SpecialHolidayPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayPay) : _paystubs.Sum(t => t.SpecialHolidayPay);
                case PaystubProperty.SpecialHolidayOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayOTPay) : _paystubs.Sum(t => t.SpecialHolidayOTPay);
                case PaystubProperty.RegularHolidayPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayPay) : _paystubs.Sum(t => t.RegularHolidayPay);
                case PaystubProperty.RegularHolidayOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayOTPay) : _paystubs.Sum(t => t.RegularHolidayOTPay);
                case PaystubProperty.RestDayNightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RestDayNightDiffPay) : _paystubs.Sum(t => t.RestDayNightDiffPay);
                case PaystubProperty.RestDayNightDiffOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RestDayNightDiffOTPay) : _paystubs.Sum(t => t.RestDayNightDiffOTPay);
                case PaystubProperty.SpecialHolidayNightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayNightDiffPay) : _paystubs.Sum(t => t.SpecialHolidayNightDiffPay);
                case PaystubProperty.SpecialHolidayNightDiffOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayNightDiffOTPay) : _paystubs.Sum(t => t.SpecialHolidayNightDiffOTPay);
                case PaystubProperty.SpecialHolidayRestDayPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayRestDayPay) : _paystubs.Sum(t => t.SpecialHolidayRestDayPay);
                case PaystubProperty.SpecialHolidayRestDayOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayRestDayOTPay) : _paystubs.Sum(t => t.SpecialHolidayRestDayOTPay);
                case PaystubProperty.SpecialHolidayRestDayNightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayRestDayNightDiffPay) : _paystubs.Sum(t => t.SpecialHolidayRestDayNightDiffPay);
                case PaystubProperty.SpecialHolidayRestDayNightDiffOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.SpecialHolidayRestDayNightDiffOTPay) : _paystubs.Sum(t => t.SpecialHolidayRestDayNightDiffOTPay);
                case PaystubProperty.RegularHolidayNightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayNightDiffPay) : _paystubs.Sum(t => t.RegularHolidayNightDiffPay);
                case PaystubProperty.RegularHolidayNightDiffOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayNightDiffOTPay) : _paystubs.Sum(t => t.RegularHolidayNightDiffOTPay);
                case PaystubProperty.RegularHolidayRestDayPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayRestDayPay) : _paystubs.Sum(t => t.RegularHolidayRestDayPay);
                case PaystubProperty.RegularHolidayRestDayOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayRestDayOTPay) : _paystubs.Sum(t => t.RegularHolidayRestDayOTPay);
                case PaystubProperty.RegularHolidayRestDayNightDiffPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayRestDayNightDiffPay) : _paystubs.Sum(t => t.RegularHolidayRestDayNightDiffPay);
                case PaystubProperty.RegularHolidayRestDayNightDiffOTPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.RegularHolidayRestDayNightDiffOTPay) : _paystubs.Sum(t => t.RegularHolidayRestDayNightDiffOTPay);
                case PaystubProperty.GrossPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.GrossPay) : _paystubs.Sum(t => t.GrossPay);
                case PaystubProperty.TotalAdjustments:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.TotalAdjustments) : _paystubs.Sum(t => t.TotalAdjustments);
                case PaystubProperty.NetPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.NetPay) : _paystubs.Sum(t => t.NetPay);
                case PaystubProperty.BasicPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.BasicPay) : _paystubs.Sum(t => t.BasicPay);
                case PaystubProperty.BasicDeductions:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.BasicDeductions) : _paystubs.Sum(t => t.BasicDeductions);
                case PaystubProperty.TotalEarningForDaily:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.TotalEarningForDaily) : _paystubs.Sum(t => t.TotalEarningForDaily);
                case PaystubProperty.AdditionalPay:
                    return _actualSwitch ? _paystubs.Sum(t => t.Actual.AdditionalPay) : _paystubs.Sum(t => t.AdditionalPay);
                case PaystubProperty.ThirteenMonthPay:
                    return _paystubs.Sum(t => t.ThirteenthMonthPay.Amount);
                default:
                    return 0;
            }
        }

        private string GetCategory(Employee employee, bool IsMinimumWageEarner)
        {
            if (employee.StartDate > _startingDate)
            {
                return SCHEDULE_74;
            }
            else if (employee.IsResigned || employee.IsTerminated)
            {
                return SCHEDULE_71;
            }
            else if (IsMinimumWageEarner)
            {
                return SCHEDULE_75;
            }
            else
            {
                return SCHEDULE_73;
            }
        }

        public bool IsSchedule71 => Category == SCHEDULE_71;
        public bool IsSchedule73 => Category == SCHEDULE_73;
        public bool IsSchedule74 => Category == SCHEDULE_74;
        public bool IsSchedule75 => Category == SCHEDULE_75;
    }

    enum PaystubProperty
    {
        TotalEarnings,
        TotalBonus,
        TotalNonTaxableAllowance,
        TotalTaxableAllowance,
        DeferredTaxableIncome,
        TaxableIncome,
        WithholdingTax,
        SssEmployeeShare,
        SssEmployerShare,
        PhilHealthEmployeeShare,
        PhilHealthEmployerShare,
        HdmfEmployeeShare,
        HdmfEmployerShare,
        TotalLoans,
        Ecola,
        GrandTotalAllowance,
        NetDeductions,
        GovernmentDeductions,
        TotalRestDayPay,
        RegularPayAndTotalRestDay,
        TotalDeductionAdjustments,
        TotalAdditionAdjustments,
        RegularPay,
        LateDeduction,
        UndertimeDeduction,
        AbsenceDeduction,
        OvertimePay,
        NightDiffPay,
        NightDiffOvertimePay,
        RestDayPay,
        RestDayOTPay,
        LeavePay,
        SpecialHolidayPay,
        SpecialHolidayOTPay,
        RegularHolidayPay,
        RegularHolidayOTPay,
        RestDayNightDiffPay,
        RestDayNightDiffOTPay,
        SpecialHolidayNightDiffPay,
        SpecialHolidayNightDiffOTPay,
        SpecialHolidayRestDayPay,
        SpecialHolidayRestDayOTPay,
        SpecialHolidayRestDayNightDiffPay,
        SpecialHolidayRestDayNightDiffOTPay,
        RegularHolidayNightDiffPay,
        RegularHolidayNightDiffOTPay,
        RegularHolidayRestDayPay,
        RegularHolidayRestDayOTPay,
        RegularHolidayRestDayNightDiffPay,
        RegularHolidayRestDayNightDiffOTPay,
        GrossPay,
        TotalAdjustments,
        NetPay,
        BasicPay,
        BasicDeductions,
        TotalEarningForDaily,
        AdditionalPay,
        ThirteenMonthPay,
    }

}
