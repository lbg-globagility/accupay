using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ReportModels;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PaystubPayslipModelDataService
    {
        private readonly int _organizationId;
        private readonly IPayPeriod _payPeriod;
        private readonly bool _isActual;

        private readonly SalaryRepository _salaryRepository;

        public PaystubPayslipModelDataService(int organizationId, IPayPeriod payPeriod, bool isActual = false)
        {
            _organizationId = organizationId;
            _payPeriod = payPeriod;
            _isActual = isActual;

            _salaryRepository = new SalaryRepository();
        }

        public async Task<List<PaystubPayslipModel>> GetData()
        {
            List<PaystubPayslipModel> paystubPayslipModels = new List<PaystubPayslipModel>();

            // TODO Create PaystubPayslipModel from database THEN check if equal to new payslip
            using (PayrollContext context = new PayrollContext())
            {
                var paystubs = await context.Paystubs.
                                        Include(p => p.Employee).Include(p => p.Actual).
                                        Where(p => p.PayPeriodID.Value == _payPeriod.RowID.Value).
                                        Where(p => p.OrganizationID.Value == _organizationId).
                                        ToListAsync();

                paystubs = paystubs.OrderBy(p => p.Employee.FullNameWithMiddleInitialLastNameFirst).ToList();

                var loans = await context.LoanTransactions.
                                        Include(l => l.LoanSchedule).
                                        Include(l => l.LoanSchedule.LoanType).
                                        Include(l => l.Paystub).
                                        Where(l => l.Paystub.PayPeriodID == _payPeriod.RowID.Value).
                                        ToListAsync();

                var adjustments = await context.Adjustments.
                                        Include(a => a.Product).
                                        Include(a => a.Paystub).
                                        Where(a => a.Paystub.PayPeriodID == _payPeriod.RowID.Value).
                                        ToListAsync();

                var actualAdjustments = await context.ActualAdjustments.
                                        Include(a => a.Product).
                                        Include(a => a.Paystub).
                                        Where(a => a.Paystub.PayPeriodID == _payPeriod.RowID.Value).
                                        ToListAsync();

                var employeeSalaries = (await _salaryRepository.GetByCutOffAsync(_organizationId,
                                                                                _payPeriod.PayFromDate)).
                                                ToList();

                var ecolas = await context.AllowanceItems.
                            Include(p => p.Allowance).
                            Include(p => p.Allowance.Product).
                            Where(p => p.Allowance.Product.PartNo.ToUpper() == ProductConstant.ECOLA.ToUpper()).
                            ToListAsync();

                foreach (var paystub in paystubs)
                {
                    var employeeId = paystub.EmployeeID.Value;

                    var employeeSalary = employeeSalaries.FirstOrDefault(s => s.EmployeeID.Value == employeeId);

                    paystub.Ecola = ecolas.
                                        Where(e => e.PaystubID.Value == paystub.RowID.Value).
                                        FirstOrDefault()?.Amount ?? 0;

                    var salary = _isActual ? employeeSalary.TotalSalary : employeeSalary.BasicSalary;

                    var allAdjustments = GetEmployeeAdjustments(actualAdjustments, employeeId);
                    allAdjustments.AddRange(GetEmployeeAdjustments(adjustments, employeeId));

                    PaystubPayslipModel paystubPayslipModel = new PaystubPayslipModel(paystub.Employee)
                    {
                        EmployeeId = employeeId,

                        EmployeeNumber = paystub.Employee?.EmployeeNo,

                        EmployeeName = paystub.Employee?.FullNameWithMiddleInitialLastNameFirst,

                        RegularPay = salary,

                        BasicHours = paystub.BasicHours,

                        Allowance = paystub.TotalAllowance - paystub.Ecola,

                        Ecola = paystub.Ecola,

                        AbsentHours = paystub.AbsentHours +
                                        (paystub.Employee.IsMonthly ? paystub.LeaveHours : 0),

                        AbsentAmount = _isActual ?
                                            paystub.Actual.AbsenceDeduction +
                                            (paystub.Employee.IsMonthly ? paystub.Actual.LeavePay : 0)
                                            :
                                            paystub.AbsenceDeduction +
                                            (paystub.Employee.IsMonthly ? paystub.LeavePay : 0),

                        LateAndUndertimeHours = paystub.LateHours + paystub.UndertimeHours,

                        LateAndUndertimeAmount = _isActual ?
                                            paystub.Actual.LateDeduction + paystub.UndertimeDeduction
                                            :
                                            paystub.LateDeduction + paystub.UndertimeDeduction,

                        LeaveHours = paystub.LeaveHours,

                        LeavePay = _isActual ? paystub.Actual.LeavePay : paystub.LeavePay,

                        GrossPay = _isActual ? paystub.Actual.GrossPay : paystub.GrossPay,

                        SSSAmount = paystub.SssEmployeeShare,

                        PhilHealthAmount = paystub.PhilHealthEmployeeShare,

                        PagibigAmount = paystub.HdmfEmployeeShare,

                        NetPay = _isActual ? paystub.Actual.NetPay : paystub.NetPay,

                        TaxWithheldAmount = paystub.WithholdingTax,

                        Loans = GetEmployeeLoans(loans, employeeId),

                        Adjustments = allAdjustments,

                        // overtimes
                        OvertimeHours = paystub.OvertimeHours,

                        OvertimePay = _isActual ? paystub.Actual.OvertimePay : paystub.OvertimePay,

                        NightDiffHours = paystub.NightDiffHours,

                        NightDiffPay = _isActual ? paystub.Actual.NightDiffPay : paystub.NightDiffPay,

                        NightDiffOvertimeHours = paystub.NightDiffOvertimeHours,

                        NightDiffOvertimePay = _isActual ? paystub.Actual.NightDiffOvertimePay :
                                                        paystub.NightDiffOvertimePay,

                        RestDayHours = paystub.RestDayHours,

                        RestDayPay = _isActual ? paystub.Actual.RestDayPay : paystub.RestDayPay,

                        RestDayOTHours = paystub.RestDayOTHours,

                        RestDayOTPay = _isActual ? paystub.Actual.RestDayOTPay : paystub.RestDayOTPay,

                        SpecialHolidayHours = paystub.SpecialHolidayHours,

                        SpecialHolidayPay = _isActual ? paystub.Actual.SpecialHolidayPay :
                                                        paystub.SpecialHolidayPay,

                        SpecialHolidayOTHours = paystub.SpecialHolidayOTHours,

                        SpecialHolidayOTPay = _isActual ? paystub.Actual.SpecialHolidayOTPay :
                                                            paystub.SpecialHolidayOTPay,

                        RegularHolidayHours = paystub.RegularHolidayHours,

                        RegularHolidayPay = _isActual ? paystub.Actual.RegularHolidayPay :
                                                        paystub.RegularHolidayPay,

                        RegularHolidayOTHours = paystub.RegularHolidayOTHours,

                        RegularHolidayOTPay = _isActual ? paystub.Actual.RegularHolidayOTPay :
                                                            paystub.RegularHolidayOTPay,

                        RestDayNightDiffHours = paystub.RestDayNightDiffHours,

                        RestDayNightDiffPay = _isActual ? paystub.Actual.RestDayNightDiffPay :
                                                            paystub.RestDayNightDiffPay,

                        RestDayNightDiffOTHours = paystub.RestDayNightDiffOTHours,

                        RestDayNightDiffOTPay = _isActual ? paystub.Actual.RestDayNightDiffOTPay :
                                                            paystub.RestDayNightDiffOTPay,

                        SpecialHolidayNightDiffHours = paystub.SpecialHolidayNightDiffHours,

                        SpecialHolidayNightDiffPay = _isActual ? paystub.Actual.SpecialHolidayNightDiffPay :
                                                                paystub.SpecialHolidayNightDiffPay,

                        SpecialHolidayNightDiffOTHours = paystub.SpecialHolidayNightDiffOTHours,

                        SpecialHolidayNightDiffOTPay = _isActual ? paystub.Actual.SpecialHolidayNightDiffOTPay :
                                                                    paystub.SpecialHolidayNightDiffOTPay,

                        SpecialHolidayRestDayHours = paystub.SpecialHolidayRestDayHours,

                        SpecialHolidayRestDayPay = _isActual ? paystub.Actual.SpecialHolidayRestDayPay :
                                                                paystub.SpecialHolidayRestDayPay,

                        SpecialHolidayRestDayOTHours = paystub.SpecialHolidayRestDayOTHours,

                        SpecialHolidayRestDayOTPay = _isActual ? paystub.Actual.SpecialHolidayRestDayOTPay :
                                                                paystub.SpecialHolidayRestDayOTPay,

                        SpecialHolidayRestDayNightDiffHours = paystub.SpecialHolidayRestDayNightDiffHours,

                        SpecialHolidayRestDayNightDiffPay = _isActual ? paystub.Actual.SpecialHolidayRestDayNightDiffPay :
                                                                        paystub.SpecialHolidayRestDayNightDiffPay,

                        SpecialHolidayRestDayNightDiffOTHours = paystub.SpecialHolidayRestDayNightDiffOTHours,

                        SpecialHolidayRestDayNightDiffOTPay = _isActual ? paystub.Actual.SpecialHolidayRestDayNightDiffOTPay :
                                                                        paystub.SpecialHolidayRestDayNightDiffOTPay,

                        RegularHolidayNightDiffHours = paystub.RegularHolidayNightDiffHours,

                        RegularHolidayNightDiffPay = _isActual ? paystub.Actual.RegularHolidayNightDiffPay :
                                                                paystub.RegularHolidayNightDiffPay,

                        RegularHolidayNightDiffOTHours = paystub.RegularHolidayNightDiffOTHours,

                        RegularHolidayNightDiffOTPay = _isActual ? paystub.Actual.RegularHolidayNightDiffOTPay :
                                                                    paystub.RegularHolidayNightDiffOTPay,

                        RegularHolidayRestDayHours = paystub.RegularHolidayRestDayHours,

                        RegularHolidayRestDayPay = _isActual ? paystub.Actual.RegularHolidayRestDayPay :
                                                                paystub.RegularHolidayRestDayPay,

                        RegularHolidayRestDayOTHours = paystub.RegularHolidayRestDayOTHours,

                        RegularHolidayRestDayOTPay = _isActual ? paystub.Actual.RegularHolidayRestDayOTPay :
                                                                paystub.RegularHolidayRestDayOTPay,

                        RegularHolidayRestDayNightDiffHours = paystub.RegularHolidayRestDayNightDiffHours,

                        RegularHolidayRestDayNightDiffPay = _isActual ? paystub.Actual.RegularHolidayRestDayNightDiffPay :
                                                                        paystub.RegularHolidayRestDayNightDiffPay,

                        RegularHolidayRestDayNightDiffOTHours = paystub.RegularHolidayRestDayNightDiffOTHours,

                        RegularHolidayRestDayNightDiffOTPay = _isActual ? paystub.Actual.RegularHolidayRestDayNightDiffOTPay :
                                                                            paystub.RegularHolidayRestDayNightDiffOTPay
                    };

                    paystubPayslipModels.Add(paystubPayslipModel.CreateSummaries(salary, paystub.BasicHours));
                }
            }

            return paystubPayslipModels;
        }

        private List<PaystubPayslipModel.Loan> GetEmployeeLoans(List<LoanTransaction> loans, int employeeId)
        {
            var employeeLoans = loans.Where(l => l.EmployeeID.Value == employeeId).ToList();

            List<PaystubPayslipModel.Loan> loanModels = new List<PaystubPayslipModel.Loan>();

            foreach (var loan in employeeLoans)

                loanModels.Add(new PaystubPayslipModel.Loan(loan));

            return loanModels;
        }

        private List<PaystubPayslipModel.Adjustment> GetEmployeeAdjustments(IEnumerable<IAdjustment> adjustments, int employeeId)
        {
            var employeeAdjustments = adjustments.
                                        Where(l => l.Paystub?.EmployeeID.Value == employeeId).
                                        ToList();

            List<PaystubPayslipModel.Adjustment> adjustmentModels = new List<PaystubPayslipModel.Adjustment>();

            foreach (var adjustment in employeeAdjustments)

                adjustmentModels.Add(new PaystubPayslipModel.Adjustment(adjustment));

            return adjustmentModels;
        }
    }
}