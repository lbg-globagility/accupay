using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
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
        // this should be replaced
        private const int DesktopUserId = 1;

        private readonly DbContextOptionsService _dbContextOptionsService;

        private readonly PayPeriodRepository _payperiodRepository;
        private readonly ICurrentUser _currentUser;

        public PayperiodService(DbContextOptionsService dbContextOptionsService, PayPeriodRepository payperiodRepository, ICurrentUser currentUser)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _payperiodRepository = payperiodRepository;
            _currentUser = currentUser;
        }

        public async Task<PayperiodDto> Start(StartPayrollDto dto)
        {
            var payperiod = await _payperiodRepository.GetByCutoffDates(dto.CutoffStart, dto.CutoffEnd);
            payperiod.Status = PayPeriodStatus.Open;

            await _payperiodRepository.Update(payperiod);

            return ConvertToDto(payperiod);
        }

        public async Task<PayrollResultDto> Calculate(PayrollResources resources, int payperiodId)
        {
            await resources.Load(payperiodId, _currentUser.OrganizationId, DesktopUserId);

            var successes = 0;
            var failures = 0;
            var details = new List<PayrollResultDetailsDto>();

            var employees = resources.Employees;
            foreach (var employee in employees)
            {
                var generation = new PayrollGeneration(_dbContextOptionsService);
                var result = generation.DoProcess(employee, resources, _currentUser.OrganizationId, DesktopUserId);

                details.Add(new PayrollResultDetailsDto
                {
                    EmployeeNo = result.EmployeeNo,
                    EmployeeName = result.FullName,
                    Status = result.Status.ToString(),
                    Description = result.Description
                });

                if (result.Status == PayrollGeneration.ResultStatus.Success)
                {
                    successes++;
                }
                else
                {
                    failures++;
                }
            }

            var resultDto = new PayrollResultDto()
            {
                Successes = successes,
                Failures = failures,
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
