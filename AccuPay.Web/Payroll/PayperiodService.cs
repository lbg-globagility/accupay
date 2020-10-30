using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayperiodService
    {
        private readonly DbContextOptionsService _dbContextOptionsService;
        private readonly PayPeriodRepository _repository;
        private readonly PayPeriodDataService _payPeriodDataService;
        private readonly PaystubDataService _paystubDataService;
        private readonly ICurrentUser _currentUser;

        public PayperiodService(
            DbContextOptionsService dbContextOptionsService,
            PayPeriodRepository repository,
            PayPeriodDataService payPeriodDataService,
            PaystubDataService paystubDataService,
            ICurrentUser currentUser)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _repository = repository;
            _payPeriodDataService = payPeriodDataService;
            _paystubDataService = paystubDataService;
            _currentUser = currentUser;
        }

        public async Task<PayrollResultDto> Calculate(PayrollResources resources, int payperiodId)
        {
            var payPeriod = await _repository.GetByIdAsync(payperiodId);

            if (payPeriod == null || payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists");

            if (payPeriod.Status != PayPeriodStatus.Open)
                throw new BusinessLogicException("Only \"Open\" pay periods can be computed.");

            await resources.Load(payperiodId, _currentUser.OrganizationId, _currentUser.UserId);

            var results = new List<PaystubEmployeeResult>();

            foreach (var employee in resources.Employees)
            {
                var generation = new PayrollGeneration(_dbContextOptionsService);
                var result = generation.DoProcess(employee, resources, _currentUser.OrganizationId, _currentUser.UserId);

                results.Add(result);
            }

            var successes = results.Where(t => t.IsSuccess).Count();
            var errors = results.Where(t => t.IsError).Count();
            var details = results
                .Select(t => new PayrollResultDetailsDto
                {
                    EmployeeId = t.EmployeeId,
                    EmployeeNo = t.EmployeeNo,
                    EmployeeName = t.FullName,
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
                createdByUserId: _currentUser.UserId);

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
            var payperiod = await _repository.GetByIdAsync(payPeriodId);

            return ConvertToDto(payperiod);
        }

        public async Task<PayPeriodDto> GetLatest()
        {
            var payperiod = await _repository.GetLatestAsync(_currentUser.OrganizationId);

            return ConvertToDto(payperiod);
        }

        public async Task<PaginatedList<PayPeriodDto>> List(PageOptions options)
        {
            var paginatedList = await _repository.GetPaginatedListAsync(options, _currentUser.OrganizationId);

            return paginatedList.Select(t => ConvertToDto(t));
        }

        public async Task<List<PayPeriodDto>> GetYearlyPayPeriods(int year)
        {
            var payPeriods = await _repository.GetYearlyPayPeriodsAsync(
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
