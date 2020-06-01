using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Payroll
{
    public class PaystubService
    {
        private readonly PaystubRepository _repository;

        public PaystubService(PaystubRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<PaystubDto>> GetByPayperiod(int payperiodId)
        {
            var paystubs = await _repository.GetByPayPeriodWithEmployeeDivisionAsync(payperiodId);
            var dtos = paystubs.Select(t => ConvertToDto(t)).ToList();

            return dtos;
        }

        private PaystubDto ConvertToDto(Paystub paystub)
        {
            var dto = new PaystubDto();
            dto.Id = paystub.RowID;
            dto.NetPay = paystub.NetPay;

            dto.Employee.FirstName = paystub.Employee.FirstName;
            dto.Employee.LastName = paystub.Employee.LastName;

            return dto;
        }
    }
}
