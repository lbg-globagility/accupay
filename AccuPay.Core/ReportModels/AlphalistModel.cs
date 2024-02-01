using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.ReportModels
{
    public class AlphalistModel
    {
        private const decimal ALLOWABLE_13TH_MONTH_PAY = 90000;
        private const decimal OTHER_FORMS_OF_COMPENSATION_CEILING = 250000;
        private const decimal BASIC_TAX_EXEMPTION = 0; //50000
        private const string SCHEDULE_71 = "Schedule 7.1";
        private const string SCHEDULE_73 = "Schedule 7.3";
        private const string SCHEDULE_74 = "Schedule 7.4";
        private const string SCHEDULE_75 = "Schedule 7.5";
        private readonly bool _actualSwitch;
        private readonly List<Paystub> _paystubs;
        
        public AlphalistModel(IGrouping<int?, Paystub> paystubs,
            TimePeriod periodRange,
            List<SalaryModel> latestSalaries,
            bool actualSwitch,
            List<WithholdingTaxBracket> withholdingTaxBrackets)
        {
            var employeeId = paystubs.Key;

            var employee = paystubs.
                FirstOrDefault(p => p.EmployeeID == employeeId).
                Employee;

            _startingDate = periodRange.Start;

            var salary = latestSalaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);
            
            //SupplementaryBAmount = _paystub;
            EmployeeID = employee.EmployeeNo;
            FirstName = employee.FirstName;
            MiddleName = employee.MiddleName;
            LastName = employee.LastName;
            TinNo = employee.TinNo;
            //RdoCode = _employee.;
            DateTime[] dates1 = { employee.StartDate, periodRange.Start };
            StartDate = dates1.Max();
            EndDate = employee.TerminationDate ?? periodRange.End;
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
            MonthlyRate = monthlyRate;
            AnnualRate = monthlyRate * 12;
            var isMinimumWage = salary?.IsMinimumWage ?? false;
            if (isMinimumWage)
            {
                MinimumWagePerDay = PayrollTools.GetDailyRate(monthlyRate,
                employee.WorkDaysPerYear);
                MinimumWagePerMonth = monthlyRate;
                IsMinimumWageEarner = isMinimumWage;
                WorkDaysPerYear = employee.WorkDaysPerYear;
            }
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

            TotalNonTaxableAllowance = GetValue(PaystubProperty.TotalNonTaxableAllowance);
            TotalTaxableAllowance = GetValue(PaystubProperty.TotalTaxableAllowance);

            SssEmployeeShare = GetValue(PaystubProperty.SssEmployeeShare);
            SssEmployerShare = GetValue(PaystubProperty.SssEmployerShare);
            PhilHealthEmployeeShare = GetValue(PaystubProperty.PhilHealthEmployeeShare);
            PhilHealthEmployerShare = GetValue(PaystubProperty.PhilHealthEmployerShare);
            HdmfEmployeeShare = GetValue(PaystubProperty.HdmfEmployeeShare);
            HdmfEmployerShare = GetValue(PaystubProperty.HdmfEmployerShare);

            TotalEmployeePremium = SssEmployeeShare + PhilHealthEmployeeShare + HdmfEmployeeShare;
            TotalEmployerPremium = SssEmployerShare + PhilHealthEmployerShare + HdmfEmployerShare;

            //var totalEmpWithholdingTax = _paystubs.Sum(t => t.WithholdingTax);
            //Category = GetCategory(employee: employee, IsMinimumWageEarner: totalEmpWithholdingTax==0);

            //GrossCompensationIncome = GetValue(PaystubProperty.GrossPay);
            GrossCompensationIncome = GetValue(PaystubProperty.BasicPay);
            if (GrossCompensationIncome < OTHER_FORMS_OF_COMPENSATION_CEILING) OtherFormsOfCompensation250K = GrossCompensationIncome;

            var basicSalary = GetValue(PaystubProperty.GrossPay) +
                GetValue(PaystubProperty.GrandTotalAllowance) +
                GetValue(PaystubProperty.TotalBonus);
            //if(salary?.IsMinimumWage ?? false)
            //{
            NonTaxableBasicSalary = GetValue(PaystubProperty.BasicPay);
            //}
            //else
            //{
            TaxableBasicSalary = GrossCompensationIncome - TotalEmployeePremium;
            //}

            TaxableIncome = GetValue(PaystubProperty.TaxableIncome);

            TotalExemptions = BASIC_TAX_EXEMPTION;
            TaxDue = GetValue(PaystubProperty.WithholdingTax);

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

             var thirteenthMonthPay = GetValue(PaystubProperty.ThirteenMonthPay);
            if(thirteenthMonthPay > ALLOWABLE_13TH_MONTH_PAY)
            {
                var inExcessThreshold = thirteenthMonthPay - ALLOWABLE_13TH_MONTH_PAY;
                var taxBracket = withholdingTaxBrackets.
                    Where(w => w.TaxableIncomeFromAmount < inExcessThreshold).
                    Where(w => inExcessThreshold <= w.TaxableIncomeToAmount).
                    FirstOrDefault();
                Taxable13thMonthPay = inExcessThreshold;
                //Taxable13thMonthPay = taxBracket.ExemptionAmount + (inExcessThreshold * taxBracket.ExemptionInExcessAmount);
                NonTaxable13thMonthPay = ALLOWABLE_13TH_MONTH_PAY;
            } else
            {
                Taxable13thMonthPay = 0M;
                NonTaxable13thMonthPay = thirteenthMonthPay;
            }

            GovernmentInsurance = GetValue(PaystubProperty.GovernmentDeductions);
            CostOfLivingAllowance = GetValue(PaystubProperty.Ecola);

            string[] separationReasons = { "terminated", "resigned", "retired", "transferred", "death" };
            var employmentStatus = EmploymentStatus.ToLower();
            if (separationReasons.Contains(employmentStatus))
            {
                if (employmentStatus == "terminated" || employmentStatus == "resigned")
                    SeparationReason = "T";
                else if (employmentStatus == "retired")
                    SeparationReason = "R";
                else if (employmentStatus == "transferred")
                    SeparationReason = "TR";
                else if (employmentStatus == "death")
                    SeparationReason = "D";
            }

            GrandTotalHolidayPay = GetValue(PaystubProperty.SpecialHolidayPay) +
                GetValue(PaystubProperty.RegularHolidayPay) +
                GetValue(PaystubProperty.SpecialHolidayRestDayPay) +
                GetValue(PaystubProperty.RegularHolidayRestDayPay);
            GrandTotalOvertimePay = GetValue(PaystubProperty.RestDayOTPay) +
                GetValue(PaystubProperty.SpecialHolidayOTPay) +
                GetValue(PaystubProperty.RegularHolidayOTPay) +
                GetValue(PaystubProperty.SpecialHolidayRestDayOTPay) +
                GetValue(PaystubProperty.RegularHolidayRestDayOTPay);
            GrandTotalNightDiffPay = GetValue(PaystubProperty.NightDiffPay) +
                GetValue(PaystubProperty.NightDiffOvertimePay) +
                GetValue(PaystubProperty.SpecialHolidayRestDayNightDiffPay) +
                GetValue(PaystubProperty.SpecialHolidayRestDayNightDiffOTPay) +
                GetValue(PaystubProperty.RegularHolidayNightDiffPay) +
                GetValue(PaystubProperty.RegularHolidayNightDiffOTPay) +
                GetValue(PaystubProperty.RegularHolidayRestDayNightDiffPay) +
                GetValue(PaystubProperty.RegularHolidayRestDayNightDiffOTPay) +
                GetValue(PaystubProperty.RestDayNightDiffPay) +
                GetValue(PaystubProperty.RestDayNightDiffOTPay) +
                GetValue(PaystubProperty.SpecialHolidayNightDiffPay) +
                GetValue(PaystubProperty.SpecialHolidayNightDiffOTPay);
        }

        private readonly DateTime _startingDate;
        public decimal OtherFormsOfCompensation250K { get; }

        public string EmployeeID { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }
        public string TinNo { get; }
        public string RdoCode { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public string Category { get; }
        public string EmploymentStatus { get; }
        public string RegisteredAddress { get; }
        public string RegisteredZipCode { get; }
        public string LocalAddress { get; }
        public string LocalZipCode { get; }
        public string ForeignAddress { get; }
        public string ForeignZipCode { get; }
        public DateTime Birthdate { get; }
        public string TelephoneNo { get; }
        public string ExemptionStatus { get; }
        public string ExemptionCode { get; }
        public string WifeClaim { get; }
        public string DependentsName { get; }
        public string DependentsBirthday { get; }
        public decimal MinimumWagePerDay { get; }
        public decimal MinimumWagePerMonth { get; }
        public bool IsMinimumWageEarner { get; }
        public decimal WorkDaysPerYear { get; }
        public string PresentEmployerTinNo { get; }
        public string PresentEmployerName { get; }
        public string PresentEmployerAddress { get; }
        public string PresentEmployerZipCode { get; }
        public string PreviousEmployerTinNo { get; }
        public string PreviousEmployerName { get; }
        public string PreviousEmployerAddress { get; }
        public string PreviousEmployerZipCode { get; }
        public decimal GrossCompensationIncome { get; }
        public decimal NonTaxableIncome => NonTaxableBasicSalary + HolidayPay + OvertimePay + NightDiffPay + HazardPay + NonTaxable13thMonthPay + DeMinimisBenefits + GovernmentInsurance + SalariesAndOtherCompensation;
        public decimal TaxableIncome { get; }
        public decimal PreviousTaxableIncome { get; }
        public decimal GrossTaxableIncome => TaxableIncome + PreviousTaxableIncome;
        public decimal TotalExemptions { get; }
        public decimal PremiumPaidOnHealth { get; }
        public decimal NetTaxableIncome {
            get
            {
                var difference = GrossTaxableIncome - TotalExemptions;
                return difference < 0 ? 0 : difference;
            }
        }
        public decimal TaxDue { get; }
        public decimal PresentTaxWithheld => TaxDue;
        public decimal PreviousTaxWithheld { get; }
        public decimal TotalTaxWithheld => PreviousTaxWithheld + PresentTaxWithheld;
        public decimal NonTaxableBasicSalary { get; }
        public decimal HolidayPay { get; }
        public decimal OvertimePay { get; }
        public decimal NightDiffPay { get; }
        public decimal HazardPay { get; }
        public decimal NonTaxable13thMonthPay { get;  }
        public decimal DeMinimisBenefits { get; }
        public decimal GovernmentInsurance { get; }
        public decimal SalariesAndOtherCompensation { get; }
        public decimal TotalNonTaxableIncome => NonTaxableBasicSalary +
            HolidayPay +
            OvertimePay +
            NightDiffPay +
            HazardPay +
            NonTaxable13thMonthPay +
            DeMinimisBenefits +
            GovernmentInsurance +
            SalariesAndOtherCompensation;
        public decimal TaxableBasicSalary { get; }
        public decimal Representation { get; }
        public decimal Transportation { get; }
        public decimal CostOfLivingAllowance { get; }
        public string SeparationReason { get; }
        public decimal FixedHousingAllowance { get; }
        public decimal OthersAName { get; }
        public decimal OthersAAmount { get; }
        public decimal OthersBName { get; }
        public decimal OthersBAmount { get; }
        public decimal Commission { get; }
        public decimal ProfitSharing { get; }
        public decimal FeesInclDirectorsFees { get; }
        public decimal Taxable13thMonthPay { get; }
        public decimal TaxableHazardPay { get; }
        public decimal TaxableOvertimePay { get; }
        public string SupplementaryAName { get; }
        public decimal SupplementaryAAmount { get; }
        public decimal SupplementaryBName { get; }
        public decimal SupplementaryBAmount { get; }
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
        public string CtcNo { get; }
        public string CtcPlace { get; }
        public string CtcDate { get; }
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

        public string EmploymentStatusLegend
        {
            get
            {
                var status = EmploymentStatus.ToLower();
                if (status == "consultant")
                    return "CO";
                else if (status == "probationary")
                    return "P";
                else if (status == "contractual" || status == "service contract" ||
                    status == "project based" || status == "project-based")
                    return "CP";
                else if (status == "regular")
                    return "R";
                else if (status == "resigned")
                    return "RE";
                else if (status == "terminated")
                    return "T";

                return string.Empty;
            }
        }

        public decimal SssEmployeeShare { get; }
        public decimal SssEmployerShare { get; }
        public decimal PhilHealthEmployeeShare { get; }
        public decimal PhilHealthEmployerShare { get; }
        public decimal HdmfEmployeeShare { get; }
        public decimal HdmfEmployerShare { get; }
        public decimal TotalEmployeePremium { get; }
        public decimal TotalEmployerPremium { get; }
        public decimal TotalNonTaxableAllowance { get; }
        public decimal TotalTaxableAllowance { get; }
        public decimal MonthlyRate { get; }
        public decimal AnnualRate { get; }
        public decimal GrandTotalHolidayPay { get; }
        public decimal GrandTotalOvertimePay { get; }
        public decimal GrandTotalNightDiffPay { get; }
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
