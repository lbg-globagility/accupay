using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    /// <summary>
    /// Takes care of loading all the information needed to produce the payroll for a given pay period.
    /// </summary>
    public class PayrollResources
    {
        private int _payPeriodId;
        private int _organizationId;
        private int _userId;

        private DateTime _payDateFrom;
        private DateTime _payDateTo;
        private TimePeriod _payPeriodSpan;

        public Product BpiInsuranceProduct { get; private set; }

        public CalendarCollection CalendarCollection { get; private set; }

        public string CurrentSystemOwner { get; private set; }

        public ListOfValueCollection ListOfValueCollection { get; private set; }

        public PayPeriod PayPeriod { get; private set; }

        public Product SickLeaveProduct { get; private set; }

        public Product VacationLeaveProduct { get; private set; }

        public IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries { get; private set; }

        public IReadOnlyCollection<Allowance> Allowances { get; private set; }

        public IReadOnlyCollection<Bonus> Bonuses { get; private set; }

        public IReadOnlyCollection<Employee> Employees { get; private set; }

        public IReadOnlyCollection<Leave> Leaves { get; private set; }

        public IReadOnlyCollection<Loan> Loans { get; private set; }

        public IReadOnlyCollection<Paystub> Paystubs { get; private set; }

        public IReadOnlyCollection<Paystub> PreviousPaystubs { get; private set; }

        public IReadOnlyCollection<Salary> Salaries { get; private set; }

        public IReadOnlyCollection<SocialSecurityBracket> SocialSecurityBrackets { get; private set; }

        public IReadOnlyCollection<TimeEntry> TimeEntries { get; private set; }

        public IReadOnlyCollection<WithholdingTaxBracket> WithholdingTaxBrackets { get; private set; }

        public readonly IPolicyHelper Policy;

        private readonly IActualTimeEntryRepository _actualTimeEntryRepository;

        private readonly IAllowanceRepository _allowanceRepository;

        private readonly IBonusRepository _bonusRepository;

        private readonly CalendarService _calendarService;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ILeaveRepository _leaveRepository;

        private readonly ListOfValueService _listOfValueService;

        private readonly ILoanRepository _loanRepository;

        private readonly IPayPeriodRepository _payPeriodRepository;

        private readonly IPaystubRepository _paystubRepository;

        private readonly IPhilHealthBracketRepository _philHealthBracketRepository;

        private readonly IProductRepository _productRepository;

        private readonly ISalaryRepository _salaryRepository;

        private readonly ISocialSecurityBracketRepository _socialSecurityBracketRepository;

        private readonly SystemOwnerService _systemOwnerService;

        private readonly ITimeEntryRepository _timeEntryRepository;

        private readonly IWithholdingTaxBracketRepository _withholdingTaxBracketRepository;

        public PayrollResources(
            IPolicyHelper policy,
            CalendarService calendarService,
            ListOfValueService listOfValueService,
            SystemOwnerService systemOwnerService,
            IActualTimeEntryRepository actualTimeEntryRepository,
            IAllowanceRepository allowanceRepository,
            IBonusRepository bonusRepository,
            IEmployeeRepository employeeRepository,
            ILeaveRepository leaveRepository,
            ILoanRepository loanRepository,
            IPayPeriodRepository payPeriodRepository,
            IPaystubRepository paystubRepository,
            IPhilHealthBracketRepository philHealthBracketRepository,
            IProductRepository productRepository,
            ISalaryRepository salaryRepository,
            ISocialSecurityBracketRepository socialSecurityBracketRepository,
            ITimeEntryRepository timeEntryRepository,
            IWithholdingTaxBracketRepository withholdingTaxBracketRepository)
        {
            Policy = policy;
            _calendarService = calendarService;
            _listOfValueService = listOfValueService;
            _systemOwnerService = systemOwnerService;
            _actualTimeEntryRepository = actualTimeEntryRepository;
            _allowanceRepository = allowanceRepository;
            _bonusRepository = bonusRepository;
            _employeeRepository = employeeRepository;
            _leaveRepository = leaveRepository;
            _loanRepository = loanRepository;
            _payPeriodRepository = payPeriodRepository;
            _paystubRepository = paystubRepository;
            _philHealthBracketRepository = philHealthBracketRepository;
            _productRepository = productRepository;
            _salaryRepository = salaryRepository;
            _socialSecurityBracketRepository = socialSecurityBracketRepository;
            _timeEntryRepository = timeEntryRepository;
            _withholdingTaxBracketRepository = withholdingTaxBracketRepository;
        }

        public async Task Load(int payPeriodId, int organizationId, int userId)
        {
            _payPeriodId = payPeriodId;
            _organizationId = organizationId;
            _userId = userId;

            // LoadPayPeriod() should be executed before LoadSocialSecurityBrackets() and LoadLoans()
            await LoadPayPeriod();

            _payDateFrom = PayPeriod.PayFromDate;
            _payDateTo = PayPeriod.PayToDate;

            _payPeriodSpan = new TimePeriod(_payDateFrom, _payDateTo);

            await LoadActualTimeEntries();
            await LoadAllowances();
            await LoadBonuses();
            await LoadBpiInsuranceProduct();
            await LoadCalendarCollection();
            await LoadEmployees();
            await LoadLeaves();
            await LoadListOfValueCollection();
            await LoadPaystubs();
            // LoadSchedules() should be executed following paystubs
            await LoadLoans();
            await LoadPreviousPaystubs();
            await LoadSalaries();
            await LoadSickLeaveProduct();
            await LoadSocialSecurityBrackets();
            await LoadSystemOwner();
            await LoadTimeEntries();
            await LoadVacationLeaveProduct();
            await LoadWithholdingTaxBrackets();
        }

        private async Task LoadAllowances()
        {
            try
            {
                Allowances = (await _allowanceRepository
                    .GetByPayPeriodWithProductAsync(
                        organizationId: _organizationId,
                        timePeriod: _payPeriodSpan))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Allowances", ex);
            }
        }

        private async Task LoadBpiInsuranceProduct()
        {
            try
            {
                BpiInsuranceProduct = await _productRepository.
                    GetOrCreateAdjustmentTypeAsync(ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                                    organizationId: _organizationId,
                                                    userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("BPI Insurance Adjustment Product", ex);
            }
        }

        private async Task LoadActualTimeEntries()
        {
            try
            {
                var datePeriod = new TimePeriod(_payDateFrom, _payDateTo);
                ActualTimeEntries = (await _actualTimeEntryRepository.
                                        GetByDatePeriodAsync(_organizationId, datePeriod)).
                                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("ActualTimeEntries", ex);
            }
        }

        private async Task LoadCalendarCollection()
        {
            var previousCutoffDateForCheckingLastWorkingDay =
                PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(_payDateFrom);

            try
            {
                var payPeriod = new TimePeriod(previousCutoffDateForCheckingLastWorkingDay, _payDateTo);

                CalendarCollection = await _calendarService.GetCalendarCollectionAsync(payPeriod);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("CalendarCollection", ex);
            }
        }

        public async Task LoadEmployees()
        {
            try
            {
                Employees = (await _employeeRepository.
                                GetAllActiveWithDivisionAndPositionAsync(_organizationId)).
                                ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Employees", ex);
            }
        }

        private async Task LoadLeaves()
        {
            try
            {
                Leaves = (await _leaveRepository.
                        GetByDatePeriodAsync(organizationId: _organizationId,
                                            datePeriod: _payPeriodSpan)).
                        ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Allowances", ex);
            }
        }

        public async Task LoadListOfValueCollection()
        {
            try
            {
                ListOfValueCollection = await _listOfValueService.CreateAsync();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("ListOfValueCollection", ex);
            }
        }

        private async Task LoadLoans()
        {
            try
            {
                // LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
                Loans = (await _loanRepository
                    .GetCurrentPayrollLoansAsync(_organizationId, PayPeriod, Paystubs))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Loans", ex);
            }
        }

        private async Task LoadPayPeriod()
        {
            try
            {
                PayPeriod = (await _payPeriodRepository.GetByIdAsync(_payPeriodId));
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("PayPeriod", ex);
            }
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
                Paystubs = (await _paystubRepository
                    .GetByPayPeriodFullPaystubAsync(_payPeriodId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Paystubs", ex);
            }
        }

        private async Task LoadPreviousPaystubs()
        {
            try
            {
                PreviousPaystubs = (await _paystubRepository
                    .GetPreviousCutOffPaystubsAsync(_payDateFrom, _organizationId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("PreviousPaystubs", ex);
            }
        }

        private async Task LoadSalaries()
        {
            try
            {
                Salaries = (await _salaryRepository
                    .GetByCutOffAsync(_organizationId, _payDateTo))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Salaries", ex);
            }
        }

        private async Task LoadSickLeaveProduct()
        {
            try
            {
                SickLeaveProduct = await _productRepository.
                    GetOrCreateLeaveTypeAsync(
                        ProductConstant.SICK_LEAVE,
                        organizationId: _organizationId,
                        userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Sick Leave Product", ex);
            }
        }

        private async Task LoadSocialSecurityBrackets()
        {
            try
            {
                // LoadPayPeriod() should be executed before LoadSocialSecurityBrackets()
                var taxEffectivityDate = new DateTime(PayPeriod.Year, PayPeriod.Month, 1);

                SocialSecurityBrackets = (await _socialSecurityBracketRepository
                    .GetByTimePeriodAsync(taxEffectivityDate))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("SocialSecurityBrackets", ex);
            }
        }

        private async Task LoadSystemOwner()
        {
            try
            {
                CurrentSystemOwner = await _systemOwnerService.GetCurrentSystemOwnerAsync();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Allowances", ex);
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

                TimeEntries = (await _timeEntryRepository.
                                    GetByDatePeriodAsync(_organizationId, datePeriod)).
                                    ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("TimeEntries", ex);
            }
        }

        private async Task LoadVacationLeaveProduct()
        {
            try
            {
                VacationLeaveProduct = await _productRepository.
                    GetOrCreateLeaveTypeAsync(ProductConstant.VACATION_LEAVE,
                                                organizationId: _organizationId,
                                                userId: _userId);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Vacation Leave Product", ex);
            }
        }

        private async Task LoadWithholdingTaxBrackets()
        {
            try
            {
                WithholdingTaxBrackets = (await _withholdingTaxBracketRepository.
                                            GetAllAsync()).
                                            ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("WithholdingTaxBrackets", ex);
            }
        }

        private async Task LoadBonuses()
        {
            try
            {
                Bonuses = (await _bonusRepository
                    .GetByPayPeriodAsync(organizationId: _organizationId, timePeriod: _payPeriodSpan))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Bonuses", ex);
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
