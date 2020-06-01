using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayrollService
    {
        private readonly DbContextOptionsService _dbContextOptionsService;

        private readonly PayPeriodRepository _payperiodRepository;

        public PayrollService(DbContextOptionsService dbContextOptionsService, PayPeriodRepository payperiodRepository)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _payperiodRepository = payperiodRepository;
        }

        public async Task<PayrollResultDto> Start(PayrollResources resources, StartPayrollDto startDto)
        {
            await resources.Load(241, 11, 1, startDto.CutoffStart, startDto.CutoffEnd);

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

        public async Task<PayrollDto> GetById(int payperiodId)
        {
            var payperiod = await _payperiodRepository.GetByIdAsync(payperiodId);

            return ConvertToDto(payperiod);
        }

        public async Task<PayrollDto> GetLatest()
        {
            var payperiod = await _payperiodRepository.GetLatest(1);

            return ConvertToDto(payperiod);
        }

        public async Task<PaginatedList<PayrollDto>> List(PageOptions options)
        {
            var (payperiods, total) = await _payperiodRepository.ListByOrganization(1, 1, options);
            var dtos = payperiods.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<PayrollDto>(dtos, total, 1, 1);
        }

        private PayrollDto ConvertToDto(PayPeriod t)
        {
            var dto = new PayrollDto()
            {
                PayperiodId = t.RowID,
                CutoffStart = t.PayFromDate,
                CutoffEnd = t.PayToDate,
                Status = t.Status.ToString()
            };

            return dto;
        }
    }
}
