using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class BonusRepository
    {
        public IEnumerable<Bonus> GetByEmployee(int employeeID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Bonuses.Include(x => x.Product).Where(x => x.EmployeeID == employeeID).ToList();
            }
        }

        public void Delete(Bonus currentBonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Bonuses.Attach(currentBonus);
                context.Bonuses.Remove(currentBonus);
                context.SaveChanges();
            }  
        }

        public void Create(Bonus bonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Bonuses.Add(bonus);
                context.SaveChanges();
            }
        }

        public void Update(Bonus bonus)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(bonus).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

    }
}
