using AccuPay.Data.Entities;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class PayPeriodRepository
    {
        public PayPeriod FindById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PayPeriods.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}