using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayperiodService
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly PayPeriodDataService _payPeriodDataService;
        private readonly PaystubDataService _paystubDataService;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PayperiodService(
            EmployeeRepository employeeRepository,
            PayPeriodRepository payPeriodrepository,
            PayPeriodDataService payPeriodDataService,
            PaystubDataService paystubDataService,
            ICurrentUser currentUser,
            IServiceScopeFactory serviceScopeFactory)
        {
            _employeeRepository = employeeRepository;
            _payPeriodRepository = payPeriodrepository;
            _payPeriodDataService = payPeriodDataService;
            _paystubDataService = paystubDataService;
            _currentUser = currentUser;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<PayrollResultDto> Calculate(PayrollResources resources, int payperiodId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payperiodId);

            if (payPeriod == null || payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            if (!payPeriod.IsOpen)
                throw new BusinessLogicException("Only \"Open\" pay periods can be computed.");

            if (resources == null)
                throw new BusinessLogicException("Failure loading resources.");

            await resources.Load(payperiodId, _currentUser.OrganizationId, _currentUser.UserId);

            var results = new List<PaystubEmployeeResult>();

            var employees = await _employeeRepository.GetAllActiveAsync(_currentUser.OrganizationId);

            foreach (var employee in employees)
            {
                if (employee?.RowID == null) continue;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var generator = scope.ServiceProvider.GetRequiredService<PayrollGenerator>();
                    var result = await generator.Start(employee.RowID.Value, resources, _currentUser.OrganizationId, _currentUser.UserId);
                    results.Add(result);
                }
            }

            var successes = results.Where(t => t.IsSuccess).Count();
            var errors = results.Where(t => t.IsError).Count();
            var details = results
                .Select(t => new PayrollResultDetailsDto
                {
                    EmployeeId = t.EmployeeId,
                    EmployeeNo = t.EmployeeNumber,
                    EmployeeName = t.EmployeeFullName,
                    Status = t.Status.ToString(),
                    Description = t.Description
                })
                .OrderBy(t => t.EmployeeName)
                .ToList();

            var resultDto = new PayrollResultDto()
            {
                Successes = successes,
                Errors = errors,
                Details = details
            };

            return resultDto;
        }

        public async Task<PayPeriodDto> Start(StartPayrollDto dto)
        {
            var newPayPeriod = await _payPeriodDataService.StartStatusAsync(
                organizationId: _currentUser.OrganizationId,
                month: dto.Month,
                year: dto.Year,
                isFirstHalf: dto.IsFirstHalf,
                currentUserId: _currentUser.UserId);

            return ConvertToDto(newPayPeriod);
        }

        public async Task Close(int payPeriodId)
        {
            await _payPeriodDataService.CloseAsync(payPeriodId: payPeriodId, userId: _currentUser.UserId);
        }

        public async Task Reopen(int payPeriodId)
        {
            await _payPeriodDataService.ReopenAsync(payPeriodId: payPeriodId, userId: _currentUser.UserId);
        }

        public async Task Delete(int payPeriodId)
        {
            await _paystubDataService.DeleteByPeriodAsync(
                payPeriodId: payPeriodId,
                userId: _currentUser.UserId,
                organizationId: _currentUser.OrganizationId);
        }

        public async Task Cancel(int payPeriodId)
        {
            await _payPeriodDataService.CancelAsync(payPeriodId: payPeriodId, userId: _currentUser.UserId);
        }

        public async Task<PayPeriodDto> GetById(int payPeriodId)
        {
            var payperiod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            return ConvertToDto(payperiod);
        }

        public async Task<PayPeriodDto> GetLatest()
        {
            var payperiod = await _payPeriodRepository.GetLatestAsync(_currentUser.OrganizationId);

            return ConvertToDto(payperiod);
        }

        public async Task<PaginatedList<PayPeriodDto>> List(PageOptions options)
        {
            var paginatedList = await _payPeriodRepository.GetPaginatedListAsync(options, _currentUser.OrganizationId);

            return paginatedList.Select(t => ConvertToDto(t));
        }

        public async Task<List<PayPeriodDto>> GetYearlyPayPeriods(int year)
        {
            var payPeriods = await _payPeriodRepository.GetYearlyPayPeriodsAsync(
                organizationId: _currentUser.OrganizationId,
                year: year,
                currentUserId: _currentUser.UserId);

            var payPeriodDtos = payPeriods.Select(x => ConvertToDto(x)).ToList();

            return payPeriodDtos;
        }

        private PayPeriodDto ConvertToDto(PayPeriod t)
        {
            if (t == null) return null;

            var dto = new PayPeriodDto();
            dto.ApplyData(t);

            return dto;
        }
    }
}
