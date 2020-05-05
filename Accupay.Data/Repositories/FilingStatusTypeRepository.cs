using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class FilingStatusTypeRepository
    {
        public async Task<IEnumerable<FilingStatusType>> GetAllAsync()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.FilingStatusTypes.ToListAsync();
            }
        }
    }
}