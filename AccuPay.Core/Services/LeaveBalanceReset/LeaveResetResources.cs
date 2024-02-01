using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Repositories;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services.LeaveBalanceReset
{
    public class LeaveResetResources : ILeaveResetResources
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveLedgerRepository _leaveLedgerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICashoutUnusedLeaveRepository _cashoutUnusedLeaveRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;
        private int _organizationId;
        private int _userId;
        private TimePeriod _timePeriod;

        public LeaveResetResources(ITimeEntryRepository timeEntryRepository,
            IEmployeeRepository employeeRepository,
            ILeaveLedgerRepository leaveLedgerRepository,
            IProductRepository productRepository,
            ICashoutUnusedLeaveRepository cashoutUnusedLeaveRepository,
            IPayPeriodRepository payPeriodRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _employeeRepository = employeeRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
            _productRepository = productRepository;
            _cashoutUnusedLeaveRepository = cashoutUnusedLeaveRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries => throw new System.NotImplementedException();

        public IReadOnlyCollection<Allowance> Allowances => throw new System.NotImplementedException();

        public IReadOnlyCollection<Bonus> Bonuses => throw new System.NotImplementedException();

        public Product BpiInsuranceProduct => throw new System.NotImplementedException();

        public CalendarCollection CalendarCollection => throw new System.NotImplementedException();

        public string CurrentSystemOwner => throw new System.NotImplementedException();

        public IReadOnlyCollection<Employee> Employees { get; internal set; }

        public IReadOnlyCollection<Leave> Leaves => throw new System.NotImplementedException();

        public ListOfValueCollection ListOfValueCollection => throw new System.NotImplementedException();

        public IReadOnlyCollection<Loan> Loans => throw new System.NotImplementedException();

        public PayPeriod PayPeriod => throw new System.NotImplementedException();

        public IReadOnlyCollection<Paystub> Paystubs => throw new System.NotImplementedException();

        public IPolicyHelper Policy => throw new System.NotImplementedException();

        public IReadOnlyCollection<Paystub> PreviousPaystubs => throw new System.NotImplementedException();

        public IReadOnlyCollection<Salary> Salaries => throw new System.NotImplementedException();

        public Product SickLeaveProduct => throw new System.NotImplementedException();

        public IReadOnlyCollection<SocialSecurityBracket> SocialSecurityBrackets => throw new System.NotImplementedException();

        public IReadOnlyCollection<TimeEntry> TimeEntries { get; internal set; }

        public Product VacationLeaveProduct => throw new System.NotImplementedException();

        public IReadOnlyCollection<WithholdingTaxBracket> WithholdingTaxBrackets => throw new System.NotImplementedException();

        public IReadOnlyCollection<Shift> Shifts => throw new System.NotImplementedException();

        public IReadOnlyCollection<LeaveLedger> LeaveLedgers { get; internal set; }

        public IReadOnlyCollection<Product> LeaveTypes { get; internal set; }

        public IReadOnlyCollection<CashoutUnusedLeave> CashoutUnusedLeaves { get; internal set; }

        public async Task Load(int organizationId,
            int userId,
            TimePeriod timePeriod)
        {
            _organizationId = organizationId;
            _userId = userId;
            _timePeriod = timePeriod;
            
            await LoadEmployees();
            await LoadTimeEntries();
            await LoadLeaveLedgers();
            await LoadLeaveTypes();
            await LoadCashoutUnusedLeaves(organizationId: organizationId);
        }

        private async Task LoadEmployees()
        {
            try
            {
                Employees = (await _employeeRepository
                    .GetAllWithinServicePeriodAsync(_organizationId, _timePeriod.Start))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("Employees", ex);
            }
        }

        private async Task LoadTimeEntries()
        {
            try
            {
                TimeEntries = (await _timeEntryRepository
                    .GetByDatePeriodAsync(_organizationId, _timePeriod))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("TimeEntries", ex);
            }
        }

        private async Task LoadLeaveLedgers()
        {
            try
            {
                LeaveLedgers = (await _leaveLedgerRepository
                    .GetAll(_organizationId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("LeaveLedgers", ex);
            }
        }


        private async Task LoadLeaveTypes()
        {
            try
            {
                LeaveTypes = (await _productRepository
                    .GetLeaveTypesAsync(_organizationId))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("LeaveTypes", ex);
            }
        }

        private async Task LoadCashoutUnusedLeaves(int organizationId)
        {
            try
            {
                var currentOpenPayPeriod = await _payPeriodRepository.GetLatestAsync(organizationId);
                CashoutUnusedLeaves = await _cashoutUnusedLeaveRepository.GetByPeriodAsync(currentOpenPayPeriod.RowID.Value);
            }
            catch (Exception ex)
            {
                throw new ResourceLoadingException("CashoutUnusedLeaves", ex);
            }
        }
    }
}
