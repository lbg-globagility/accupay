using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayperiodService
    {
        private readonly DbContextOptionsService _dbContextOptionsService;

        private readonly PayPeriodRepository _payperiodRepository;

        public PayperiodService(DbContextOptionsService dbContextOptionsService, PayPeriodRepository payperiodRepository)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _payperiodRepository = payperiodRepository;
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
            await resources.Load(payperiodId, 11, 1);

            var successes = 0;
            var failures = 0;

            var employees = resources.Employees;
            foreach (var employee in employees)
            {
                var generation = new PayrollGeneration(_dbContextOptionsService);
                var result = generation.DoProcess(employee, resources, 1, 1);

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
                Failures = failures
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
            var payperiod = await _payperiodRepository.GetLatest(1);

            return ConvertToDto(payperiod);
        }

        public async Task<PaginatedList<PayperiodDto>> List(PageOptions options)
        {
            var (payperiods, total) = await _payperiodRepository.ListByOrganization(1, 1, options);
            var dtos = payperiods.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<PayperiodDto>(dtos, total, 1, 1);
        }

        private PayperiodDto ConvertToDto(PayPeriod t)
        {
            var dto = new PayperiodDto()
            {
                Id = t.RowID,
                CutoffStart = t.PayFromDate,
                CutoffEnd = t.PayToDate,
                Status = t.Status.ToString()
            };

            return dto;
        }
    }
}
