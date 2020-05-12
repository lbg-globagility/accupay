using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubActualRepository
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