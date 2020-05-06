using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    /// <summary>
    /// Takes care of loading all the information needed to produce the payroll for a given pay period.
    /// </summary>
    public class PayrollResources
    {
        private readonly int _payPeriodId;
        private readonly int _organizationId;
        private readonly int _userId;

        private readonly DateTime _payDateFrom;
        private readonly DateTime _payDateTo;
        private readonly TimePeriod _payPeriodSpan;

        public ListOfValueCollection ListOfValueCollection { get; private set; }

        public ICollection<Employee> Employees { get; private set; }

        public ICollection<TimeEntry> TimeEntries { get; private set; }

        public ICollection<LoanSchedule> LoanSchedules { get; private set; }

        public ICollection<LoanTransaction> LoanTransactions { get; private set; }

        public ICollection<Salary> Salaries { get; private set; }

        public ICollection<Paystub> Paystubs { get; private set; }

        public ICollection<Paystub> PreviousPaystubs { get; private set; }

        public IReadOnlyCollection<PhilHealthBracket> PhilHealthBrackets { get; private set; }

        public IReadOnlyCollection<SocialSecurityBracket> SocialSecurityBrackets { get; private set; }

        public IReadOnlyCollection<WithholdingTaxBracket> WithholdingTaxBrackets { get; private set; }

        public PayPeriod PayPeriod { get; private set; }

        public ICollection<Allowance> Allowances { get; private set; }

        public ICollection<ActualTimeEntry> ActualTimeEntries { get; private set; }

        public IReadOnlyCollection<Leave> Leaves { get; private set; }

        public IReadOnlyCollection<FilingStatusType> FilingStatuses { get; private set; }

        public IReadOnlyCollection<DivisionMinimumWage> DivisionMinimumWages { get; private set; }

        public SystemOwnerService SystemOwner { get; private set; }

        public CalendarCollection CalendarCollection { get; private set; }

        public Product BpiInsuranceProduct { get; private set; }

        public Product SickLeaveProduct { get; private set; }

        public Product VacationLeaveProduct { get; private set; }

        public PayrollResources(int payPeriodId,
                                int organizationId,
                                int userId,
                                DateTime payDateFrom,
                                DateTime payDateTo)
        {
            _payPeriodId = payPeriodId;
            _organizationId = organizationId;
            _userId = userId;
            _payDateFrom = payDateFrom;
            _payDateTo = payDateTo;

            _payPeriodSpan = new TimePeriod(_payDateFrom, _payDateTo);
        }

        public async Task Load()
        {
            // LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
            await LoadPayPeriod();

            // LoadSettings() should be executed before LoadCalendarCollection()
            await LoadListOfValueCollection();

            await Task.WhenAll(new[] {
                    LoadSystemOwner(),
                    LoadEmployees(),
                    LoadLoanSchedules(),
                    LoadLoanTransactions(),
                    LoadSalaries(),
                    LoadPaystubs(),
                    LoadPreviousPaystubs(),
                    LoadSocialSecurityBrackets(),
                    LoadPhilHealthBrackets(),
                    LoadWithholdingTaxBrackets(),
                    LoadAllowances(),
                    LoadTimeEntries(),
                    LoadActualTimeEntries(),
                    LoadFilingStatuses(),
                    LoadDivisionMinimumWages(),
                    LoadCalendarCollection(),
                    LoadBpiInsuranceProduct(),
                    LoadSickLeaveProduct(),
                    LoadVacationLeaveProduct(),
                    LoadLeaves()
                });
        }

        /// <summary>
        /// This are the current paystubs that will be updated when the payroll is generated.
        /// On the first generation or when payroll is deleted, this list will be empty.
        /// </summary>
        /// <returns></returns>
        private async Task LoadPaystubs()
        {
            try
            {
                Paystubs = (await new PaystubRepository().
                                GetByPayPeriodFullPaystubAsync(_payPeriodId)).
                                ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Paystubs", ex);
            }
        }

        private async Task LoadPayPeriod()
        {
            try
            {
                PayPeriod = (await new PayPeriodRepository().GetByIdAsync(_payPeriodId));
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("PayPeriod", ex);
            }
        }

        public async Task LoadListOfValueCollection()
        {
            try
            {
                ListOfValueCollection = await ListOfValueCollection.CreateAsync();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("ListOfValueCollection", ex);
            }
        }

        public async Task LoadSystemOwner()
        {
            try
            {
                await Task.Run(() =>
                {
                    SystemOwner = new SystemOwnerService();
                });
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("SystemOwner", ex);
            }
        }

        public async Task LoadEmployees()
        {
            try
            {
                Employees = (await new EmployeeRepository().
                                GetAllActiveWithDivisionAndPositionAsync(_organizationId)).
                                ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Employees", ex);
            }
        }

        private async Task LoadTimeEntries()
        {
            var previousCutoffDateForCheckingLastWorkingDay =
                    PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(_payDateFrom);

            try
            {
                var datePeriod = new TimePeriod(previousCutoffDateForCheckingLastWorkingDay,
                                                _payDateTo);

                TimeEntries = (await new TimeEntryRepository().
                                    GetByDatePeriodAsync(_organizationId, datePeriod)).
                                    ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("TimeEntries", ex);
            }
        }

        private async Task LoadCalendarCollection()
        {
            var previousCutoffDateForCheckingLastWorkingDay =
                    PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(_payDateFrom);

            try
            {
                await Task.Run(() =>
                {
                    var calculationBasis = ListOfValueCollection.
                        GetEnum("Pay rate.CalculationBasis", PayRateCalculationBasis.Organization);

                    var payPeriod = new TimePeriod(previousCutoffDateForCheckingLastWorkingDay, _payDateTo);

                    CalendarCollection = PayrollTools.
                            GetCalendarCollection(payPeriod, calculationBasis, _organizationId);
                });
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("EmployeeDutySchedules", ex);
            }
        }

        private async Task LoadActualTimeEntries()
        {
            try
            {
                var datePeriod = new TimePeriod(_payDateFrom, _payDateTo);
                ActualTimeEntries = (await new ActualTimeEntryRepository().
                                        GetByDatePeriodAsync(_organizationId, datePeriod)).
                                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("ActualTimeEntries", ex);
            }
        }

        private async Task LoadLoanSchedules()
        {
            try
            {
                LoanSchedules = (await new LoanScheduleRepository().
                                    GetCurrentPayrollLoansAsync(_organizationId, _payDateTo)).
                                    ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("LoanSchedules", ex);
            }
        }

        private async Task LoadLoanTransactions()
        {
            try
            {
                // TODO: get this from paystub
                LoanTransactions = (await new LoanTransactionRepository().
                                        GetByPayPeriodAsync(_payPeriodId)).
                                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("LoanTransactions", ex);
            }
        }

        private async Task LoadSalaries()
        {
            try
            {
                Salaries = (await new SalaryRepository().
                                GetByCutOffAsync(_organizationId, _payDateFrom)).
                                ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Salaries", ex);
            }
        }

        private async Task LoadPreviousPaystubs()
        {
            try
            {
                PreviousPaystubs = (await new PaystubRepository().
                                        GetPreviousCutOffPaystubsAsync(_payDateFrom, _organizationId)).
                                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("PreviousPaystubs", ex);
            }
        }

        private async Task LoadSocialSecurityBrackets()
        {
            try
            {
                // LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
                var taxEffectivityDate = new DateTime(PayPeriod.Year, PayPeriod.Month, 1);

                SocialSecurityBrackets = (await new SocialSecurityBracketRepository().
                                            GetByTimePeriodAsync(taxEffectivityDate)).
                                            ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("SocialSecurityBrackets", ex);
            }
        }

        private async Task LoadPhilHealthBrackets()
        {
            try
            {
                PhilHealthBrackets = (await new PhilHealthBracketRepository().GetAllAsync()).ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("PhilHealthBrackets", ex);
            }
        }

        private async Task LoadWithholdingTaxBrackets()
        {
            try
            {
                WithholdingTaxBrackets = (await new WithholdingTaxBracketRepository().
                                            GetAllAsync()).
                                            ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("WithholdingTaxBrackets", ex);
            }
        }

        private async Task LoadAllowances()
        {
            try
            {
                var allowanceRepo = new AllowanceRepository();

                Allowances = await (allowanceRepo.
                                GetByPayPeriodWithProductAsync(organizationId: _organizationId,
                                                                timePeriod: _payPeriodSpan));
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Allowances", ex);
            }
        }

        private async Task LoadFilingStatuses()
        {
            try
            {
                FilingStatuses = (await new FilingStatusTypeRepository().GetAllAsync()).ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Filing Statuses", ex);
            }
        }

        private async Task LoadDivisionMinimumWages()
        {
            try
            {
                DivisionMinimumWages = (await new DivisionMinimumWageRepository().
                                            GetByDateAsync(_organizationId, _payDateTo)).
                                            ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("DivisionMinimumWage", ex);
            }
        }

        private async Task LoadBpiInsuranceProduct()
        {
            try
            {
                BpiInsuranceProduct = await new ProductRepository().
                    GetOrCreateAdjustmentTypeAsync(ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                                    organizationId: _organizationId,
                                                    userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("BPI Insurance Adjustment Product", ex);
            }
        }

        private async Task LoadSickLeaveProduct()
        {
            try
            {
                SickLeaveProduct = await new ProductRepository().
                    GetOrCreateLeaveTypeAsync(ProductConstant.SICK_LEAVE,
                                                organizationId: _organizationId,
                                                userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Sick Leave Product", ex);
            }
        }

        private async Task LoadVacationLeaveProduct()
        {
            try
            {
                VacationLeaveProduct = await new ProductRepository().
                    GetOrCreateLeaveTypeAsync(ProductConstant.VACATION_LEAVE,
                                                organizationId: _organizationId,
                                                userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Vacation Leave Product", ex);
            }
        }

        private async Task LoadLeaves()
        {
            try
            {
                Leaves = (await new LeaveRepository().
                        GetByTimePeriodAsync(organizationId: _organizationId,
                                            timePeriod: _payPeriodSpan)).
                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Allowances", ex);
            }
        }
    }

    public class ResourceLoadingException : Exception
    {
        public ResourceLoadingException(string resource, Exception ex) :
                base($"Failure to load resource `{resource}`", ex)
        {
        }
    }
}