using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubActualRepository
    {
        public async Task<PaystubActual> GetByIdAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PaystubActuals.FirstOrDefaultAsync(x => x.RowID == id);
            }
        }
    }
}