using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PaystubActualRepository : IPaystubActualRepository
    {
        private readonly PayrollContext _context;

        public PaystubActualRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<PaystubActual> GetByIdAsync(int id)
        {
            return await _context.PaystubActuals.FirstOrDefaultAsync(x => x.RowID == id);
        }
    }
}
