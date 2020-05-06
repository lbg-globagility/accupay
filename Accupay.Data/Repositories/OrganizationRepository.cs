using AccuPay.Data.Entities;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class OrganizationRepository
    {
        public Organization GetById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Organizations.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}