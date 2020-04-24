using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class PaystubRepository
    {
        public IEnumerable<Paystub> List
        {
            get
            {
                using (PayrollContext context = new PayrollContext())
                {
                    return context.Paystubs.ToList();
                }
            }
        }

        public Paystub First()
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Paystubs.FirstOrDefault();
            }
        }

        public Paystub FindById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Paystubs.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}