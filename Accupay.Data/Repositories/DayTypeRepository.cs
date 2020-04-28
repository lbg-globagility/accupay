using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class DayTypeRepository
    {
        public async Task<ICollection<DayType>> GetAll()
        {
            using (var context = new PayrollContext())
            {
                return await context.DayTypes.ToListAsync();
            }
        }

        public async Task Save(DayType dayType)
        {
            using (var context = new PayrollContext())
            {
                if (dayType.RowID == null)
                    context.DayTypes.Add(dayType);
                else
                    context.Entry(dayType).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}