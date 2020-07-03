using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayperiodService
    {
        private readonly DbContextOptionsService _dbContextOptionsService;

        private readonly PayPeriodRepository _payperiodRepository;
        private readonly ICurrentUser _currentUser;

        public PayperiodService(
            DbContextOptionsService dbContextOptionsService,
            PayPeriodRepository payperiodRepository,
            ICurrentUser currentUser)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _payperiodRepository = payperiodRepository;
            _currentUser = currentUser;
        }

        public async Task<PayperiodDto> Start(StartPayrollDto dto)
        {
            var payperiod = await _payperiodRepository.GetByCutoffDates(
                new TimePeriod(dto.CutoffStart, dto.CutoffEnd),
                _currentUser.OrganizationId);

            payperiod.Status = PayPeriodStatus.Open;
            payperiod.LastUpdBy = _currentUser.DesktopUserId;

            await _payperiodRepository.Update(payperiod);

            return ConvertToDto(payperiod);
        }

        public async Task<PayrollResultDto> Calculate(PayrollResources resources, int payperiodId)
        {
            await resources.Load(payperiodId, _currentUser.OrganizationId, _currentUser.DesktopUserId);

            var results = new List<PayrollGeneration.Result>();

            foreach (var employee in resources.Employees)
            {
                var generation = new PayrollGeneration(_dbContextOptionsService);
                var result = generation.DoProcess(employee, resources, _currentUser.OrganizationId, _currentUser.DesktopUserId);

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

        public async Task<PayperiodDto> GetById(int payperiodId)
        {
            var payperiod = await _payperiodRepository.GetByIdAsync(payperiodId);

            return ConvertToDto(payperiod);
        }

        public async Task<PayperiodDto> GetLatest()
        {
            var payperiod = await _payperiodRepository.GetLatest(_currentUser.OrganizationId);

            return ConvertToDto(payperiod);
        }

        public async Task<PaginatedList<PayperiodDto>> List(PageOptions options)
        {
            var paginatedList = await _payperiodRepository.GetPaginatedListAsync(options, _currentUser.OrganizationId);
            var dtos = paginatedList.List.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<PayperiodDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        private PayperiodDto ConvertToDto(PayPeriod t)
        {
            if (t == null) return null;

            var dto = new PayperiodDto();
            dto.ApplyData(t);

            return dto;
        }
    }
}
