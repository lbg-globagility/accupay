using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Repositories
{
    public class BreakTimeBracketRepository
    {
        public IEnumerable<BreakTimeBracket> GetAll(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.BreakTimeBrackets.
                                Include(x => x.Division).
                                Where(x => x.Division.OrganizationID == organizationId).
                                ToList();
            }
        }
    }
}