using Accupay.Data.Entities;
using System.Linq;

namespace Accupay.Data.Repositories
{
    public class OrganizationRepository
    {
        public Organization FindById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.Organizations.FirstOrDefault(x => x.RowID == id);
            }
        }
    }
}