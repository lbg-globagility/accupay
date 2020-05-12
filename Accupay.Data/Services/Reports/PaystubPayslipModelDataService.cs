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
        private readonly PayrollContext _context;
        private readonly SalaryRepository _salaryRepository;

        public PaystubPayslipModelDataService(PayrollContext context, SalaryRepository salaryRepository)
        {
            _context = context;
            _salaryRepository = salaryRepository;
        }

        public async Task<List<PaystubPayslipModel>> GetData(int organizationId, IPayPeriod payPeriod, bool isActual = false)
        {
            List<PaystubPayslipModel> paystubPayslipModels = new List<PaystubPayslipModel>();

            // TODO Create PaystubPayslipModel from database THEN check if equal to new payslip
            // use paystub repository GetFullPaystub
            var paystubs = await _context.Paystubs.
                                    Include(p => p.Employee).Include(p => p.Actual).
                                    Where(p => p.PayPeriodID == payPeriod.RowID).
                                    Where(p => p.OrganizationID == organizationId).
                                    ToListAsync();

            paystubs = paystubs.OrderBy(p => p.Employee.FullNameWithMiddleInitialLastNameFirst).ToList();

            var loans = await _context.LoanTransactions.
                                    Include(l => l.LoanSchedule).
                                    Include(l => l.LoanSchedule.LoanType).
                                    Include(l => l.Paystub).
                                    Where(l => l.Paystub.PayPeriodID == payPeriod.RowID).
                                    ToListAsync();

            var adjustments = await _context.Adjustments.
                                    Include(a => a.Product).
                                    Include(a => a.Paystub).
                                    Where(a => a.Paystub.PayPeriodID == payPeriod.RowID).
                                    ToListAsync();

            var actualAdjustments = await _context.ActualAdjustments.
                                    Include(a => a.Product).
                                    Include(a => a.Paystub).
                                    Where(a => a.Paystub.PayPeriodID == payPeriod.RowID).
                                    ToListAsync();

            var employeeSalaries = (await _salaryRepository.GetByCutOffAsync(organizationId,
                                                                            payPeriod.PayFromDate)).
                                            ToList();

            var ecolas = await _context.AllowanceItems.
                        Include(p => p.Allowance).
                        Include(p => p.Allowance.Product).
                        Where(p => p.Allowance.Product.PartNo.ToUpper() == ProductConstant.ECOLA.ToUpper()).
                        ToListAsync();

            foreach (var paystub in paystubs)
            {
                var employeeId = paystub.EmployeeID.Value;

                var employeeSalary = employeeSalaries.FirstOrDefault(s => s.EmployeeID == employeeId);

                paystub.Ecola = ecolas.
                                    Where(e => e.PaystubID == paystub.RowID).
                                    FirstOrDefault()?.Amount ?? 0;

                var salary = isActual ? employeeSalary.TotalSalary : employeeSalary.BasicSalary;

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

                    AbsentAmount = isActual ?
                                        paystub.Actual.AbsenceDeduction +
                                        (paystub.Employee.IsMonthly ? paystub.Actual.LeavePay : 0)
                                        :
                                        paystub.AbsenceDeduction +
                                        (paystub.Employee.IsMonthly ? paystub.LeavePay : 0),

                    LateAndUndertimeHours = paystub.LateHours + paystub.UndertimeHours,

                    LateAndUndertimeAmount = isActual ?
                                        paystub.Actual.LateDeduction + paystub.UndertimeDeduction
                                        :
                                        paystub.LateDeduction + paystub.UndertimeDeduction,

                    LeaveHours = paystub.LeaveHours,

                    LeavePay = isActual ? paystub.Actual.LeavePay : paystub.LeavePay,

                    GrossPay = isActual ? paystub.Actual.GrossPay : paystub.GrossPay,

                    SSSAmount = paystub.SssEmployeeShare,

                    PhilHealthAmount = paystub.PhilHealthEmployeeShare,

                    PagibigAmount = paystub.HdmfEmployeeShare,

                    NetPay = isActual ? paystub.Actual.NetPay : paystub.NetPay,

                    TaxWithheldAmount = paystub.WithholdingTax,

                    Loans = GetEmployeeLoans(loans, employeeId),

                    Adjustments = allAdjustments,

                    // overtimes
                    OvertimeHours = paystub.OvertimeHours,

                    OvertimePay = isActual ? paystub.Actual.OvertimePay : paystub.OvertimePay,

                    NightDiffHours = paystub.NightDiffHours,

                    NightDiffPay = isActual ? paystub.Actual.NightDiffPay : paystub.NightDiffPay,

                    NightDiffOvertimeHours = paystub.NightDiffOvertimeHours,

                    NightDiffOvertimePay = isActual ? paystub.Actual.NightDiffOvertimePay :
                                                    paystub.NightDiffOvertimePay,

                    RestDayHours = paystub.RestDayHours,

                    RestDayPay = isActual ? paystub.Actual.RestDayPay : paystub.RestDayPay,

                    RestDayOTHours = paystub.RestDayOTHours,

                    RestDayOTPay = isActual ? paystub.Actual.RestDayOTPay : paystub.RestDayOTPay,

                    SpecialHolidayHours = paystub.SpecialHolidayHours,

                    SpecialHolidayPay = isActual ? paystub.Actual.SpecialHolidayPay :
                                                    paystub.SpecialHolidayPay,

                    SpecialHolidayOTHours = paystub.SpecialHolidayOTHours,

                    SpecialHolidayOTPay = isActual ? paystub.Actual.SpecialHolidayOTPay :
                                                        paystub.SpecialHolidayOTPay,

                    RegularHolidayHours = paystub.RegularHolidayHours,

                    RegularHolidayPay = isActual ? paystub.Actual.RegularHolidayPay :
                                                    paystub.RegularHolidayPay,

                    RegularHolidayOTHours = paystub.RegularHolidayOTHours,

                    RegularHolidayOTPay = isActual ? paystub.Actual.RegularHolidayOTPay :
                                                        paystub.RegularHolidayOTPay,

                    RestDayNightDiffHours = paystub.RestDayNightDiffHours,

                    RestDayNightDiffPay = isActual ? paystub.Actual.RestDayNightDiffPay :
                                                        paystub.RestDayNightDiffPay,

                    RestDayNightDiffOTHours = paystub.RestDayNightDiffOTHours,

                    RestDayNightDiffOTPay = isActual ? paystub.Actual.RestDayNightDiffOTPay :
                                                        paystub.RestDayNightDiffOTPay,

                    SpecialHolidayNightDiffHours = paystub.SpecialHolidayNightDiffHours,

                    SpecialHolidayNightDiffPay = isActual ? paystub.Actual.SpecialHolidayNightDiffPay :
                                                            paystub.SpecialHolidayNightDiffPay,

                    SpecialHolidayNightDiffOTHours = paystub.SpecialHolidayNightDiffOTHours,

                    SpecialHolidayNightDiffOTPay = isActual ? paystub.Actual.SpecialHolidayNightDiffOTPay :
                                                                paystub.SpecialHolidayNightDiffOTPay,

                    SpecialHolidayRestDayHours = paystub.SpecialHolidayRestDayHours,

                    SpecialHolidayRestDayPay = isActual ? paystub.Actual.SpecialHolidayRestDayPay :
                                                            paystub.SpecialHolidayRestDayPay,

                    SpecialHolidayRestDayOTHours = paystub.SpecialHolidayRestDayOTHours,

                    SpecialHolidayRestDayOTPay = isActual ? paystub.Actual.SpecialHolidayRestDayOTPay :
                                                            paystub.SpecialHolidayRestDayOTPay,

                    SpecialHolidayRestDayNightDiffHours = paystub.SpecialHolidayRestDayNightDiffHours,

                    SpecialHolidayRestDayNightDiffPay = isActual ? paystub.Actual.SpecialHolidayRestDayNightDiffPay :
                                                                    paystub.SpecialHolidayRestDayNightDiffPay,

                    SpecialHolidayRestDayNightDiffOTHours = paystub.SpecialHolidayRestDayNightDiffOTHours,

                    SpecialHolidayRestDayNightDiffOTPay = isActual ? paystub.Actual.SpecialHolidayRestDayNightDiffOTPay :
                                                                    paystub.SpecialHolidayRestDayNightDiffOTPay,

                    RegularHolidayNightDiffHours = paystub.RegularHolidayNightDiffHours,

                    RegularHolidayNightDiffPay = isActual ? paystub.Actual.RegularHolidayNightDiffPay :
                                                            paystub.RegularHolidayNightDiffPay,

                    RegularHolidayNightDiffOTHours = paystub.RegularHolidayNightDiffOTHours,

                    RegularHolidayNightDiffOTPay = isActual ? paystub.Actual.RegularHolidayNightDiffOTPay :
                                                                paystub.RegularHolidayNightDiffOTPay,

                    RegularHolidayRestDayHours = paystub.RegularHolidayRestDayHours,

                    RegularHolidayRestDayPay = isActual ? paystub.Actual.RegularHolidayRestDayPay :
                                                            paystub.RegularHolidayRestDayPay,

                    RegularHolidayRestDayOTHours = paystub.RegularHolidayRestDayOTHours,

                    RegularHolidayRestDayOTPay = isActual ? paystub.Actual.RegularHolidayRestDayOTPay :
                                                            paystub.RegularHolidayRestDayOTPay,

                    RegularHolidayRestDayNightDiffHours = paystub.RegularHolidayRestDayNightDiffHours,

                    RegularHolidayRestDayNightDiffPay = isActual ? paystub.Actual.RegularHolidayRestDayNightDiffPay :
                                                                    paystub.RegularHolidayRestDayNightDiffPay,

                    RegularHolidayRestDayNightDiffOTHours = paystub.RegularHolidayRestDayNightDiffOTHours,

                    RegularHolidayRestDayNightDiffOTPay = isActual ? paystub.Actual.RegularHolidayRestDayNightDiffOTPay :
                                                                        paystub.RegularHolidayRestDayNightDiffOTPay
                };

                paystubPayslipModels.Add(paystubPayslipModel.CreateSummaries(salary, paystub.BasicHours));
            }

            return paystubPayslipModels;
        }

        private List<PaystubPayslipModel.Loan> GetEmployeeLoans(List<LoanTransaction> loans, int employeeId)
        {
            var employeeLoans = loans.Where(l => l.EmployeeID == employeeId).ToList();

            List<PaystubPayslipModel.Loan> loanModels = new List<PaystubPayslipModel.Loan>();

            foreach (var loan in employeeLoans)

                loanModels.Add(new PaystubPayslipModel.Loan(loan));

            return loanModels;
        }

        private List<PaystubPayslipModel.Adjustment> GetEmployeeAdjustments(IEnumerable<IAdjustment> adjustments, int employeeId)
        {
            var employeeAdjustments = adjustments.
                                        Where(l => l.Paystub?.EmployeeID == employeeId).
                                        ToList();

            List<PaystubPayslipModel.Adjustment> adjustmentModels = new List<PaystubPayslipModel.Adjustment>();

            foreach (var adjustment in employeeAdjustments)

                adjustmentModels.Add(new PaystubPayslipModel.Adjustment(adjustment));

            return adjustmentModels;
        }
    }
}