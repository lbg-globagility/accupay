using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class PayrollGeneration
    {
        private PayrollContext _context;

        private SystemOwnerService _systemOwnerService;

        //private static ILog logger = LogManager.GetLogger("PayrollLogger");

        private Employee _employee;

        private PayrollResources _resources;

        private int _organizationId;

        private int _userId;

        private Salary _salary;

        private ICollection<LoanSchedule> _loanSchedules;

        private ICollection<LoanTransaction> _loanTransactions;

        private ICollection<TimeEntry> _previousTimeEntries;

        private PayPeriod _payPeriod;

        private ListOfValueCollection _settings;

        private ICollection<TimeEntry> _timeEntries;

        private ICollection<Allowance> _allowances;

        private ICollection<AllowanceItem> _allowanceItems = new List<AllowanceItem>();

        private ICollection<ActualTimeEntry> _actualtimeentries;

        private IReadOnlyCollection<Leave> _leaves;

        private Paystub _previousPaystub;

        private Product _bpiInsuranceProduct;

        private Product _sickLeaveProduct;

        private Product _vacationLeaveProduct;

        private CalendarCollection _calendarCollection;

        private Paystub _paystub;

        public PayrollGeneration(PayrollContext context, SystemOwnerService systemOwnerService)
        {
            this._context = context;
            _systemOwnerService = systemOwnerService;
        }

        public Result DoProcess(Employee employee,
                                PayrollResources resources,
                                int organizationId,
                                int userId)
        {
            _employee = employee;

            _organizationId = organizationId;

            _userId = userId;

            _resources = resources;

            _settings = resources.ListOfValueCollection;

            _payPeriod = resources.PayPeriod;

            _bpiInsuranceProduct = resources.BpiInsuranceProduct;

            _sickLeaveProduct = resources.SickLeaveProduct;

            _vacationLeaveProduct = resources.VacationLeaveProduct;

            _calendarCollection = resources.CalendarCollection;

            _salary = resources.Salaries.FirstOrDefault(s => s.EmployeeID == _employee.RowID);

            _paystub = resources.Paystubs.FirstOrDefault(p => p.EmployeeID == _employee.RowID);

            _previousPaystub = resources.PreviousPaystubs.FirstOrDefault(p => p.EmployeeID == _employee.RowID);

            _loanSchedules = resources.LoanSchedules.
                                        Where(l => l.EmployeeID == _employee.RowID).
                                        ToList();

            _loanTransactions = resources.LoanTransactions.
                                        Where(t => t.EmployeeID == _employee.RowID).
                                        ToList();

            _previousTimeEntries = resources.TimeEntries.
                                        Where(t => t.EmployeeID == _employee.RowID).
                                        ToList();

            _timeEntries = resources.TimeEntries.
                                        Where(t => t.EmployeeID == _employee.RowID).
                                        Where(t => _payPeriod.PayFromDate <= t.Date).
                                        Where(t => t.Date <= _payPeriod.PayToDate).
                                        OrderBy(t => t.Date).
                                        ToList();

            _actualtimeentries = resources.ActualTimeEntries.
                                        Where(t => t.EmployeeID == _employee.RowID).
                                        ToList();

            _allowances = resources.Allowances.
                                        Where(a => a.EmployeeID == _employee.RowID).
                                        ToList();

            _leaves = resources.Leaves.
                                    Where(a => a.EmployeeID == _employee.RowID).
                                    ToList();
            try
            {
                GeneratePayStub();

                return new Result(_employee.EmployeeNo,
                            _employee.FullNameWithMiddleInitialLastNameFirst,
                            ResultStatus.Success,
                            "");
            }
            catch (Exception ex)
            {
                //logger.Error("DoProcess", ex);

                return new Result(_employee.EmployeeNo,
                                _employee.FullNameWithMiddleInitialLastNameFirst,
                                ResultStatus.Error,
                                ex.Message);
            }
        }

        private void GeneratePayStub()
        {
            try
            {
                if (_salary == null)
                    throw new PayrollException("Employee has no salary for this cutoff.");

                if ((!_timeEntries.Any()) && (_employee.IsDaily || _employee.IsMonthly))
                    throw new PayrollException("No time entries.");

                if (_employee.Position == null)
                    throw new PayrollException("Employee has no job position set.");

                if (_paystub == null)
                {
                    _paystub = new Paystub()
                    {
                        OrganizationID = _organizationId,
                        Created = DateTime.Now,
                        CreatedBy = _userId,
                        LastUpdBy = _userId,
                        EmployeeID = _employee.RowID,
                        PayPeriodID = _payPeriod.RowID,
                        PayFromdate = _payPeriod.PayFromDate,
                        PayToDate = _payPeriod.PayToDate
                    };

                    _paystub.Actual = new PaystubActual()
                    {
                        OrganizationID = _organizationId,
                        EmployeeID = _employee.RowID,
                        PayPeriodID = _payPeriod.RowID,
                        PayFromDate = _payPeriod.PayFromDate,
                        PayToDate = _payPeriod.PayToDate
                    };
                }

                _paystub.EmployeeID = _employee.RowID;

                var newLoanTransactions = ComputePayroll();

                SavePayroll(newLoanTransactions);
            }
            catch (PayrollException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failure to generate paystub for employee {_paystub.EmployeeID}", ex);
            }
        }

        public void SavePayroll(List<LoanTransaction> newLoanTransactions)
        {
            if (_paystub.RowID.HasValue)
            {
                _context.Entry(_paystub).State = EntityState.Modified;
                _context.Entry(_paystub.Actual).State = EntityState.Modified;

                if (_paystub.ThirteenthMonthPay != null)
                    _context.Entry(_paystub.ThirteenthMonthPay).State = EntityState.Modified;
            }
            else
                _context.Paystubs.Add(_paystub);

            if (EligibleForNewBPIInsurance())
            {
                _context.Adjustments.Add(new Adjustment()
                {
                    OrganizationID = _organizationId,
                    CreatedBy = _userId,
                    Created = DateTime.Now,
                    Paystub = _paystub,
                    ProductID = _bpiInsuranceProduct.RowID,
                    Amount = -_employee.BPIInsurance
                });
            }

            _context.Set<AllowanceItem>().RemoveRange(_paystub.AllowanceItems);

            _paystub.AllowanceItems = _allowanceItems;

            foreach (var newLoanTransaction in newLoanTransactions)
            {
                _context.LoanTransactions.Add(newLoanTransaction);
            }

            if (_systemOwnerService.GetCurrentSystemOwner() != SystemOwnerService.Benchmark)
            {
                UpdateLeaveLedger(_context);
                UpdatePaystubItems(_context);
            }
            else
                UpdateBenchmarkLeaveLedger(_context);

            _context.SaveChanges();
        }

        public List<LoanTransaction> ComputePayroll(Paystub paystub = null,
                                                    ICollection<AllowanceItem> allowanceItems = null)
        {
            List<LoanTransaction> newLoanTransactions = new List<LoanTransaction>();

            if (paystub != null)
                _paystub = paystub;

            if (_systemOwnerService.GetCurrentSystemOwner() != SystemOwnerService.Benchmark)
            {
                ComputeBasicHoursAndPay();

                ComputeHours();

                ComputeTotalEarnings();
            }

            // Allowance
            CalculateAllowances(allowanceItems);
            var grandTotalAllowance = _paystub.TotalAllowance + _paystub.TotalTaxableAllowance;

            // Loans
            newLoanTransactions = ComputeLoans();

            // gross pay and total earnings should be higher than the goverment deduction calculators
            // since it is sometimes used in computing the basis pay for the deductions
            // depending on the organization's policy
            if (_paystub.TotalEarnings < 0)
                _paystub.TotalEarnings = 0;

            _paystub.GrossPay = _paystub.TotalEarnings + _paystub.TotalBonus + grandTotalAllowance;

            _paystub.TotalAdjustments = _paystub.Adjustments.Sum(a => a.Amount);
            // BPI Insurance feature, currently used by LA Global
            if (EligibleForNewBPIInsurance())
                _paystub.TotalAdjustments -= _employee.BPIInsurance;

            var socialSecurityCalculator = new SssCalculator(_settings, _resources.SocialSecurityBrackets);
            socialSecurityCalculator.Calculate(_paystub, _previousPaystub, _salary, _employee, _payPeriod, _systemOwnerService);

            var philHealthCalculator = new PhilHealthCalculator(new PhilHealthPolicy(_settings), _resources.PhilHealthBrackets);
            philHealthCalculator.Calculate(_salary, _paystub, _previousPaystub, _employee, _payPeriod, _allowances, _systemOwnerService);

            var hdmfCalculator = new HdmfCalculator();
            hdmfCalculator.Calculate(_salary, _paystub, _employee, _payPeriod, _settings);

            var withholdingTaxCalculator = new WithholdingTaxCalculator(_settings, _resources.FilingStatuses, _resources.WithholdingTaxBrackets, _resources.DivisionMinimumWages);
            withholdingTaxCalculator.Calculate(_paystub, _previousPaystub, _employee, _payPeriod, _salary);

            _paystub.NetPay = AccuMath.CommercialRound(_paystub.GrossPay - _paystub.NetDeductions + _paystub.TotalAdjustments);

            var actualCalculator = new PaystubActualCalculator();
            actualCalculator.Compute(_employee, _salary, _settings, _payPeriod, _paystub, _systemOwnerService);

            var thirteenthMonthPayCalculator = new ThirteenthMonthPayCalculator(
                                                        organizationId: _organizationId,
                                                        userId: _userId);
            thirteenthMonthPayCalculator.Calculate(_employee, _paystub, _timeEntries, _actualtimeentries, _salary, _settings, _allowanceItems, _systemOwnerService);

            return newLoanTransactions;
        }

        private bool EligibleForNewBPIInsurance()
        {
            return _settings.GetBoolean("Employee Policy.UseBPIInsurance", false) && IsFirstPaystub() && _employee.BPIInsurance > 0 && !_paystub.Adjustments.Any(a => a.Product?.PartNo == ProductConstant.BPI_INSURANCE_ADJUSTMENT);
        }

        private bool IsFirstPaystub()
        {
            var query = _context.Paystubs.Where(p => p.EmployeeID == _employee.RowID);

            var paystubCount = query.Count();

            if (paystubCount > 1)
                return false;
            else if (paystubCount == 0)
                return true;
            else
            {
                var firstPaystub = query.OrderBy(p => p.PayFromdate).FirstOrDefault();

                if (firstPaystub == null || firstPaystub.PayPeriodID == _payPeriod.RowID)
                    return true;

                return false;
            }
        }

        private void ComputeTotalEarnings()
        {
            if (_employee.IsFixed)
                _paystub.TotalEarnings = _paystub.BasicPay + _paystub.AdditionalPay;
            else if (_employee.IsMonthly)
            {
                var isFirstPayAsDailyRule = _settings.GetBoolean("Payroll Policy", "isfirstsalarydaily");

                var isFirstPay = _payPeriod.PayFromDate <= _employee.StartDate &&
                                    _employee.StartDate <= _payPeriod.PayToDate;

                if (isFirstPay && isFirstPayAsDailyRule)
                {
                    _paystub.TotalEarnings = _paystub.RegularPay +
                                            _paystub.LeavePay +
                                            _paystub.AdditionalPay;
                }
                else
                {
                    _paystub.RegularHours = _paystub.BasicHours - _paystub.LeaveHours;

                    _paystub.RegularPay = _paystub.BasicPay - _paystub.LeavePay;

                    _paystub.TotalEarnings = (_paystub.BasicPay + _paystub.AdditionalPay) -
                                            _paystub.BasicDeductions;
                }
            }
            else if (_employee.IsDaily)
            {
                _paystub.TotalEarnings = _paystub.RegularPay +
                                        _paystub.LeavePay +
                                        _paystub.AdditionalPay;
            }
        }

        private void ComputeBasicHoursAndPay()
        {
            if (_employee.IsMonthly || _employee.IsFixed)
            {
                if (_employee.WorkDaysPerYear > 0)
                {
                    var workDaysPerPayPeriod = _employee.WorkDaysPerYear /
                                                CalendarConstants.MonthsInAYear /
                                                CalendarConstants.SemiMonthlyPayPeriodsPerMonth;

                    _paystub.BasicHours = workDaysPerPayPeriod * 8;
                }

                _paystub.BasicPay = AccuMath.CommercialRound((_salary.BasicSalary / 2), 2);
            }
            else if (_employee.IsDaily)
            {
                ComputeBasicHoursForDay();

                _paystub.BasicPay = _timeEntries.Sum(t => t.BasicDayPay);
            }
        }

        private void ComputeBasicHoursForDay()
        {
            var basicHours = 0M;

            foreach (var timeEntry in _timeEntries)
            {
                var payrateCalendar = _calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);

                if (!(timeEntry.IsRestDay ||
                    (timeEntry.TotalLeaveHours > 0) ||
                    payrate.IsRegularHoliday ||
                    payrate.IsSpecialNonWorkingHoliday))

                    basicHours += timeEntry.WorkHours;
            }

            _paystub.BasicHours = basicHours;
        }

        private void ComputeHours()
        {
            PaystubRate paystubRate = new PaystubRate();
            paystubRate.Compute(_timeEntries, _salary, _employee, _actualtimeentries);

            _paystub.RegularHours = paystubRate.RegularHours;
            _paystub.RegularPay = paystubRate.RegularPay;
            _paystub.Actual.RegularPay = paystubRate.ActualRegularPay;

            _paystub.OvertimeHours = paystubRate.OvertimeHours;
            _paystub.OvertimePay = paystubRate.OvertimePay;
            _paystub.Actual.OvertimePay = paystubRate.ActualOvertimePay;

            _paystub.NightDiffHours = paystubRate.NightDiffHours;
            _paystub.NightDiffPay = paystubRate.NightDiffPay;
            _paystub.Actual.NightDiffPay = paystubRate.ActualNightDiffPay;

            _paystub.NightDiffOvertimeHours = paystubRate.NightDiffOvertimeHours;
            _paystub.NightDiffOvertimePay = paystubRate.NightDiffOvertimePay;
            _paystub.Actual.NightDiffOvertimePay = paystubRate.ActualNightDiffOvertimePay;

            _paystub.RestDayHours = paystubRate.RestDayHours;
            _paystub.RestDayPay = paystubRate.RestDayPay;
            _paystub.Actual.RestDayPay = paystubRate.ActualRestDayPay;

            _paystub.RestDayOTHours = paystubRate.RestDayOTHours;
            _paystub.RestDayOTPay = paystubRate.RestDayOTPay;
            _paystub.Actual.RestDayOTPay = paystubRate.ActualRestDayOTPay;

            _paystub.SpecialHolidayHours = paystubRate.SpecialHolidayHours;
            _paystub.SpecialHolidayPay = paystubRate.SpecialHolidayPay;
            _paystub.Actual.SpecialHolidayPay = paystubRate.ActualSpecialHolidayPay;

            _paystub.SpecialHolidayOTHours = paystubRate.SpecialHolidayOTHours;
            _paystub.SpecialHolidayOTPay = paystubRate.SpecialHolidayOTPay;
            _paystub.Actual.SpecialHolidayOTPay = paystubRate.ActualSpecialHolidayOTPay;

            _paystub.RegularHolidayHours = paystubRate.RegularHolidayHours;
            _paystub.RegularHolidayPay = paystubRate.RegularHolidayPay;
            _paystub.Actual.RegularHolidayPay = paystubRate.ActualRegularHolidayPay;

            _paystub.RegularHolidayOTHours = paystubRate.RegularHolidayOTHours;
            _paystub.RegularHolidayOTPay = paystubRate.RegularHolidayOTPay;
            _paystub.Actual.RegularHolidayOTPay = paystubRate.ActualRegularHolidayOTPay;

            _paystub.LeaveHours = paystubRate.LeaveHours;
            _paystub.LeavePay = paystubRate.LeavePay;
            _paystub.Actual.LeavePay = paystubRate.ActualLeavePay;

            _paystub.LateHours = paystubRate.LateHours;
            _paystub.LateDeduction = paystubRate.LateDeduction;
            _paystub.Actual.LateDeduction = paystubRate.ActualLateDeduction;

            _paystub.UndertimeHours = paystubRate.UndertimeHours;
            _paystub.UndertimeDeduction = paystubRate.UndertimeDeduction;
            _paystub.Actual.UndertimeDeduction = paystubRate.ActualUndertimeDeduction;

            _paystub.AbsentHours = paystubRate.AbsentHours;
            _paystub.AbsenceDeduction = paystubRate.AbsenceDeduction;
            _paystub.Actual.AbsenceDeduction = paystubRate.ActualAbsenceDeduction;
        }

        private void CalculateAllowances(ICollection<AllowanceItem> allowanceItems = null)
        {
            if (allowanceItems == null)
                CreateAllowanceItems();
            else
            {
                _allowanceItems.Clear();

                foreach (var item in allowanceItems)

                    _allowanceItems.Add(item);
            }

            ComputeTotalAllowances();
        }

        private void CreateAllowanceItems()
        {
            var dailyCalculator = new DailyAllowanceCalculator(
                                                            _settings,
                                                            _calendarCollection,
                                                            _previousTimeEntries,
                                                            organizationId: _organizationId,
                                                            userId: _userId);

            var semiMonthlyCalculator = new SemiMonthlyAllowanceCalculator(
                                                            new AllowancePolicy(_settings),
                                                            _employee,
                                                            _paystub,
                                                            _payPeriod,
                                                            _calendarCollection,
                                                            _timeEntries,
                                                            organizationId: _organizationId,
                                                            userId: _userId);

            foreach (var allowance in _allowances)
            {
                var item = AllowanceItem.Create(paystub: _paystub,
                                                product: allowance.Product,
                                                payperiodId: _payPeriod.RowID.Value,
                                                allowanceId: allowance.RowID.Value,
                                                organizationId: _organizationId,
                                                userId: _userId);

                if (allowance.IsOneTime)
                {
                    item.Amount = allowance.Amount;
                }
                else if (allowance.IsDaily)
                {
                    item = dailyCalculator.Compute(_payPeriod, allowance, _employee, _paystub, _timeEntries);
                }
                else if (allowance.IsSemiMonthly)
                {
                    item = semiMonthlyCalculator.Calculate(allowance);
                }
                else if (allowance.IsMonthly)
                {
                    // TODO: monthly payrolls are only for Fixed allowances only.
                    if (allowance.Product.Fixed && _payPeriod.IsEndOfTheMonth)
                    {
                        item.Amount = allowance.Amount;
                    }
                }
                else
                {
                    item = null;
                }

                _allowanceItems.Add(item);
            }
        }

        private void ComputeTotalAllowances()
        {
            _paystub.TotalTaxableAllowance = AccuMath.CommercialRound(_allowanceItems.Where(a => a.IsTaxable).Sum(a => a.Amount));

            _paystub.TotalAllowance = AccuMath.CommercialRound(_allowanceItems.Where(a => !a.IsTaxable).Sum(a => a.Amount));
        }

        private void UpdateBenchmarkLeaveLedger(PayrollContext context)
        {
            var vacationLedger = context.LeaveLedgers.
                                        Include(l => l.Product).
                                        Include(l => l.LastTransaction).
                                        Where(l => l.EmployeeID == _employee.RowID).
                                        Where(l => l.Product.IsVacationLeave).
                                        FirstOrDefault();

            vacationLedger.LeaveTransactions = new List<LeaveTransaction>();

            if (vacationLedger == null)
                throw new Exception($"Vacation ledger for Employee No.: {_employee.EmployeeNo}");

            UpdateLedgerTransaction(employeeId: _employee.RowID.Value,
                                    leaveId: null, ledger:
                                    vacationLedger, totalLeaveHours:
                                    _paystub.LeaveHours,
                                    transactionDate: _payPeriod.PayToDate);
        }

        private void UpdateLeaveLedger(PayrollContext context)
        {
            var leaves = _leaves.
                            Where(l => l.EmployeeID == _employee.RowID).
                            OrderBy(l => l.StartDate).
                            ToList();

            var leaveIds = leaves.Select(l => l.RowID);

            var transactions = (from t in context.LeaveTransactions
                                where leaveIds.Contains(t.ReferenceID)
                                select t).ToList();

            var employeeId = _employee.RowID;
            var ledgers = context.LeaveLedgers.
                                Include(x => x.Product).
                                Include(x => x.LeaveTransactions).
                                Include(x => x.LastTransaction).
                                Where(x => x.EmployeeID == employeeId).
                                ToList();

            var newLeaveTransactions = new List<LeaveTransaction>();
            foreach (var leave in leaves)
            {
                // If a transaction has already been made for the current leave, skip the current leave.
                if (transactions.Any(t => t.ReferenceID == leave.RowID))
                    continue;
                else
                {
                    var ledger = ledgers.FirstOrDefault(l => l.Product.PartNo == leave.LeaveType);

                    // retrieves the time entries within leave date range
                    var timeEntry = _timeEntries.
                                    Where(t => leave.StartDate == t.Date);

                    if (timeEntry == null)
                        continue;

                    // summate the leave hours
                    var totalLeaveHours = timeEntry.Sum(t => t.TotalLeaveHours);

                    if (totalLeaveHours == 0)
                        continue;

                    var transactionDate = leave.EndDate ?? leave.StartDate;

                    UpdateLedgerTransaction(employeeId: leave.EmployeeID.Value,
                                            leaveId: leave.RowID,
                                            ledger: ledger,
                                            totalLeaveHours: totalLeaveHours,
                                            transactionDate: transactionDate);
                }
            }
        }

        private void UpdateLedgerTransaction(int employeeId,
                                            int? leaveId,
                                            LeaveLedger ledger,
                                            decimal totalLeaveHours,
                                            DateTime transactionDate)
        {
            var newTransaction = new LeaveTransaction()
            {
                OrganizationID = _organizationId,
                Created = DateTime.Now,
                EmployeeID = employeeId,
                PayPeriodID = _payPeriod.RowID,
                ReferenceID = leaveId,
                TransactionDate = transactionDate,
                Type = LeaveTransactionType.Debit,
                Amount = totalLeaveHours,
                Paystub = _paystub,
                Balance = (ledger?.LastTransaction?.Balance ?? 0) - totalLeaveHours
            };

            ledger.LeaveTransactions.Add(newTransaction);
            ledger.LastTransaction = newTransaction;
        }

        private List<LoanTransaction> ComputeLoans()
        {
            var newLoanTransactions = new List<LoanTransaction>();

            if (_loanTransactions.Count > 0)
                _paystub.TotalLoans = _loanTransactions.Sum(t => t.Amount);
            else
            {
                string[] acceptedLoans = new string[] { };
                if (_payPeriod.IsFirstHalf)
                    acceptedLoans = new[] { "Per pay period", "First half" };
                else if (_payPeriod.IsEndOfTheMonth)
                    acceptedLoans = new[] { "Per pay period", "End of the month" };

                var loanSchedules = _loanSchedules.Where(l => acceptedLoans.Contains(l.DeductionSchedule)).ToList();

                foreach (var loanSchedule in loanSchedules)
                {
                    var loanTransaction = new LoanTransaction()
                    {
                        Created = DateTime.Now,
                        LastUpd = DateTime.Now,
                        OrganizationID = _organizationId,
                        EmployeeID = _paystub.EmployeeID,
                        PayPeriodID = _payPeriod.RowID,
                        LoanScheduleID = loanSchedule.RowID.Value,
                        LoanPayPeriodLeft = loanSchedule.LoanPayPeriodLeft == null ? 0 :
                                                            (Convert.ToInt32(loanSchedule.LoanPayPeriodLeft) - 1)
                    };

                    if (loanSchedule.DeductionAmount > loanSchedule.TotalBalanceLeft)
                        loanTransaction.Amount = loanSchedule.TotalBalanceLeft;
                    else
                        loanTransaction.Amount = loanSchedule.DeductionAmount;

                    loanTransaction.TotalBalance = loanSchedule.TotalBalanceLeft - loanTransaction.Amount;

                    newLoanTransactions.Add(loanTransaction);
                }

                _paystub.TotalLoans = newLoanTransactions.Aggregate(0M, (total, x) => x.Amount + total);
            }

            return newLoanTransactions;
        }

        private void UpdatePaystubItems(PayrollContext context)
        {
            context.Entry(_paystub).Collection(p => p.PaystubItems).Load();
            context.Set<PaystubItem>().RemoveRange(_paystub.PaystubItems);

            var vacationLeaveBalance = context.PaystubItems.Where(p => p.Product.PartNo == ProductConstant.VACATION_LEAVE).Where(p => Convert.ToBoolean(p.Paystub.RowID == _paystub.RowID)).FirstOrDefault();

            var vacationLeaveUsed = _timeEntries.Sum(t => t.VacationLeaveHours);
            var newBalance = _employee.LeaveBalance - vacationLeaveUsed;

            vacationLeaveBalance = new PaystubItem()
            {
                OrganizationID = _organizationId,
                Created = DateTime.Now,
                CreatedBy = _userId,
                ProductID = _vacationLeaveProduct.RowID,
                PayAmount = newBalance,
                Paystub = _paystub
            };

            _paystub.PaystubItems.Add(vacationLeaveBalance);

            var sickLeaveBalance = context.PaystubItems.Where(p => p.Product.PartNo == ProductConstant.SICK_LEAVE).Where(p => Convert.ToBoolean(p.Paystub.RowID == _paystub.RowID)).FirstOrDefault();

            var sickLeaveUsed = _timeEntries.Sum(t => t.SickLeaveHours);
            var newBalance2 = _employee.SickLeaveBalance - sickLeaveUsed;

            sickLeaveBalance = new PaystubItem()
            {
                OrganizationID = _organizationId,
                ProductID = _sickLeaveProduct.RowID,
                PayAmount = newBalance2,
                Paystub = _paystub
            };

            _paystub.PaystubItems.Add(sickLeaveBalance);
        }

        private class PayFrequency
        {
            public const int SemiMonthly = 1;
            public const int Monthly = 2;
        }

        private class SalaryType
        {
            public const string Fixed = "Fixed";
            public const string Monthly = "Monthly";
            public const string Daily = "Daily";
        }

        public class Result
        {
            public string EmployeeNo { get; set; }

            public string FullName { get; set; }

            public ResultStatus Status { get; set; }

            public string Description { get; set; }

            public Result(string employeeNo, string fullName, ResultStatus status, string description)
            {
                this.EmployeeNo = employeeNo;
                this.FullName = fullName;
                this.Status = status;
                this.Description = description;
            }
        }

        private class PaystubRate : IPaystubRate
        {
            public decimal RegularHours { get; set; }
            public decimal RegularPay { get; set; }
            public decimal ActualRegularPay { get; set; }

            public decimal OvertimeHours { get; set; }
            public decimal OvertimePay { get; set; }
            public decimal ActualOvertimePay { get; set; }

            public decimal NightDiffHours { get; set; }
            public decimal NightDiffPay { get; set; }
            public decimal ActualNightDiffPay { get; set; }

            public decimal NightDiffOvertimeHours { get; set; }
            public decimal NightDiffOvertimePay { get; set; }
            public decimal ActualNightDiffOvertimePay { get; set; }

            public decimal RestDayHours { get; set; }
            public decimal RestDayPay { get; set; }
            public decimal ActualRestDayPay { get; set; }

            public decimal RestDayOTHours { get; set; }
            public decimal RestDayOTPay { get; set; }
            public decimal ActualRestDayOTPay { get; set; }

            public decimal SpecialHolidayHours { get; set; }
            public decimal SpecialHolidayPay { get; set; }
            public decimal ActualSpecialHolidayPay { get; set; }

            public decimal SpecialHolidayOTHours { get; set; }
            public decimal SpecialHolidayOTPay { get; set; }
            public decimal ActualSpecialHolidayOTPay { get; set; }

            public decimal RegularHolidayHours { get; set; }
            public decimal RegularHolidayPay { get; set; }
            public decimal ActualRegularHolidayPay { get; set; }

            public decimal RegularHolidayOTHours { get; set; }
            public decimal RegularHolidayOTPay { get; set; }
            public decimal ActualRegularHolidayOTPay { get; set; }

            public decimal HolidayPay { get; set; }

            public decimal LeaveHours { get; set; }
            public decimal LeavePay { get; set; }
            public decimal ActualLeavePay { get; set; }

            public decimal LateHours { get; set; }
            public decimal LateDeduction { get; set; }
            public decimal ActualLateDeduction { get; set; }

            public decimal UndertimeHours { get; set; }
            public decimal UndertimeDeduction { get; set; }
            public decimal ActualUndertimeDeduction { get; set; }

            public decimal AbsentHours { get; set; }
            public decimal AbsenceDeduction { get; set; }
            public decimal ActualAbsenceDeduction { get; set; }

            public void Compute(ICollection<TimeEntry> timeEntries, Salary salary, Employee employee, ICollection<ActualTimeEntry> actualtimeentries)
            {
                var totalTimeEntries = TotalTimeEntryCalculator.Calculate(timeEntries, salary, employee, actualtimeentries);

                this.RegularHours = totalTimeEntries.RegularHours;
                this.RegularPay = totalTimeEntries.RegularPay;
                this.ActualRegularPay = totalTimeEntries.ActualRegularPay;

                this.OvertimeHours = totalTimeEntries.OvertimeHours;
                this.OvertimePay = totalTimeEntries.OvertimePay;
                this.ActualOvertimePay = totalTimeEntries.ActualOvertimePay;

                this.NightDiffHours = totalTimeEntries.NightDiffHours;
                this.NightDiffPay = totalTimeEntries.NightDiffPay;
                this.ActualNightDiffPay = totalTimeEntries.ActualNightDiffPay;

                this.NightDiffOvertimeHours = totalTimeEntries.NightDiffOvertimeHours;
                this.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;
                this.ActualNightDiffOvertimePay = totalTimeEntries.ActualNightDiffOvertimePay;

                this.RestDayHours = totalTimeEntries.RestDayHours;
                this.RestDayPay = totalTimeEntries.RestDayPay;
                this.ActualRestDayPay = totalTimeEntries.ActualRestDayPay;

                this.RestDayOTHours = totalTimeEntries.RestDayOTHours;
                this.RestDayOTPay = totalTimeEntries.RestDayOTPay;
                this.ActualRestDayOTPay = totalTimeEntries.ActualRestDayOTPay;

                this.SpecialHolidayHours = totalTimeEntries.SpecialHolidayHours;
                this.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;
                this.ActualSpecialHolidayPay = totalTimeEntries.ActualSpecialHolidayPay;

                this.SpecialHolidayOTHours = totalTimeEntries.SpecialHolidayOTHours;
                this.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;
                this.ActualSpecialHolidayOTPay = totalTimeEntries.ActualSpecialHolidayOTPay;

                this.RegularHolidayHours = totalTimeEntries.RegularHolidayHours;
                this.RegularHolidayPay = totalTimeEntries.RegularHolidayPay;
                this.ActualRegularHolidayPay = totalTimeEntries.ActualRegularHolidayPay;

                this.RegularHolidayOTHours = totalTimeEntries.RegularHolidayOTHours;
                this.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
                this.ActualRegularHolidayOTPay = totalTimeEntries.ActualRegularHolidayOTPay;

                this.HolidayPay = totalTimeEntries.HolidayPay;

                this.LeaveHours = totalTimeEntries.LeaveHours;
                this.LeavePay = totalTimeEntries.LeavePay;
                this.ActualLeavePay = totalTimeEntries.ActualLeavePay;

                this.LateHours = totalTimeEntries.LateHours;
                this.LateDeduction = totalTimeEntries.LateDeduction;
                this.ActualLateDeduction = totalTimeEntries.ActualLateDeduction;

                this.UndertimeHours = totalTimeEntries.UndertimeHours;
                this.UndertimeDeduction = totalTimeEntries.UndertimeDeduction;
                this.ActualUndertimeDeduction = totalTimeEntries.ActualUndertimeDeduction;

                this.AbsentHours = totalTimeEntries.AbsentHours;
                this.AbsenceDeduction = totalTimeEntries.AbsenceDeduction;
                this.ActualAbsenceDeduction = totalTimeEntries.ActualAbsenceDeduction;
            }
        }

        public enum ResultStatus
        {
            Success,
            Warning,
            Error
        }
    }
}