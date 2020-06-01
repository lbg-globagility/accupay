using AccuPay.Data.Services;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PayrollService
    {
        private readonly DbContextOptionsService _dbContextOptionsService;

        public PayrollService(DbContextOptionsService dbContextOptionsService)
        {
            _dbContextOptionsService = dbContextOptionsService;
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
    }
}
